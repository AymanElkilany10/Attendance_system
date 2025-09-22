using BussinessLogic.Models._AttendanceDto;
using BussinessLogic.Models._ManagerSummary;
using BussinessLogic.ServicesAbstraction;
using DataAccess.Data._UnitOfWork;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BussinessLogic.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Attendance_SystemContext _context;

        public AttendanceService(IUnitOfWork unitOfWork, Attendance_SystemContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<EmployeeAttendanceReportDto?> GetEmployeeAttendanceReport(int employeeId)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                return null;

            var records = await GetAttendanceRecordsFromSP(employeeId);

            foreach (var record in records)
            {
                record.DayDeduction = record.IsAbsent
                    ? 1m
                    : (record.CheckIn.HasValue ? CalculateDayDeduction(record.CheckIn.Value) : 1m);
            }
            ApplyMonthlyAbsenceRules(records);
            ApplyYearlyAbsenceRules(records, employee, out decimal totalDeduction);

            return new EmployeeAttendanceReportDto
            {
                EmployeeId = employee.Id,
                FullName = $"{employee.Fname} {employee.Lname}",
                TotalDays = records.Count,
                DaysPresent = records.Count(a => !a.IsAbsent),
                DaysAbsent = records.Count(a => a.IsAbsent),
                DaysLate = records.Count(a => a.IsLate),
                TotalDeduction = totalDeduction,
                Records = records
            };
        }

        public async Task<List<EmployeeAttendanceReportDto>> GetReportsForManager(int managerId)
        {
            var manager = await _unitOfWork.EmployeeRepository.GetByIdAsync(managerId);

            if (manager == null || !manager.Is_Manager)
                return new List<EmployeeAttendanceReportDto>();
            

            var connection = _context.Database.GetDbConnection();
            var reportsDict = new Dictionary<int, EmployeeAttendanceReportDto>();

            await using var command = connection.CreateCommand();
            command.CommandText = "GetAttendanceReportsByManager";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@ManagerId", SqlDbType.Int) { Value = managerId });

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                int empId = reader.GetInt32(0);
                string fullName = reader.GetString(1);

                var record = new AttendanceRecordDto
                {
                    Date = DateOnly.FromDateTime(reader.GetDateTime(2)),
                    CheckIn = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                    CheckOut = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                    IsLate = reader.GetBoolean(5),
                    IsAbsent = reader.GetBoolean(6),
                    Status = reader.GetString(7),
                    DayDeduction = 0m
                };

                if (!reportsDict.TryGetValue(empId, out var report))
                {
                    report = new EmployeeAttendanceReportDto
                    {
                        EmployeeId = empId,
                        FullName = fullName,
                        Records = new List<AttendanceRecordDto>()
                    };
                    reportsDict.Add(empId, report);
                }

                report.Records.Add(record);
            }

            foreach (var report in reportsDict.Values)
            {
                foreach (var rec in report.Records)
                {
                    rec.DayDeduction = rec.IsAbsent
                        ? 1m
                        : (rec.CheckIn.HasValue ? CalculateDayDeduction(rec.CheckIn.Value) : 1m);
                }
                ApplyMonthlyAbsenceRules(report.Records);
                ApplyYearlyAbsenceRules(report.Records, null, out decimal totalDeduction);

                report.TotalDays = report.Records.Count;
                report.DaysPresent = report.Records.Count(r => !r.IsAbsent);
                report.DaysAbsent = report.Records.Count(r => r.IsAbsent);
                report.DaysLate = report.Records.Count(r => r.IsLate);
                report.TotalDeduction = totalDeduction;
            }

            return reportsDict.Values.ToList();
        }



        public async Task<List<LineManagerTeamSummaryDto>> GetLineManagerSummariesByDepartmentManagerAsync(
        int departmentManagerId, DateTime startDate, DateTime endDate)
        {
            var deptManager = await _unitOfWork.EmployeeRepository.GetByIdAsync(departmentManagerId);
            if (deptManager == null || !deptManager.Is_Manager)
                return new List<LineManagerTeamSummaryDto>();

            var allEmployees = await _unitOfWork.EmployeeRepository.GetAllASync();

            var lineManagers = allEmployees
                .Where(e => e.Line_Manager_Id == departmentManagerId && e.Is_Manager)
                .ToList();

            var summaries = new List<LineManagerTeamSummaryDto>();

            foreach (var lm in lineManagers)
            {
                var reports = await GetReportsForManager(lm.Id);
                foreach (var rep in reports)
                {
                    rep.Records = rep.Records
                        .Where(r => r.Date.ToDateTime(TimeOnly.MinValue) >= startDate &&
                                    r.Date.ToDateTime(TimeOnly.MinValue) <= endDate)
                        .ToList();
                }

                int totalEmployees = reports.Count;
                int totalWorkingDays = reports.Sum(r => r.Records.Count);
                int totalAbsentDays = reports.Sum(r => r.Records.Count(r2 => r2.IsAbsent));
                int totalLateDays = reports.Sum(r => r.Records.Count(r2 => r2.IsLate));

                decimal percentAbsent = totalWorkingDays == 0
                    ? 0
                    : (decimal)totalAbsentDays / totalWorkingDays * 100m;

                decimal percentLate = totalWorkingDays == 0
                    ? 0
                    : (decimal)totalLateDays / totalWorkingDays * 100m;

                summaries.Add(new LineManagerTeamSummaryDto
                {
                    LineManagerId = lm.Id,
                    LineManagerName = $"{lm.Fname} {lm.Lname}",
                    TotalEmployees = totalEmployees,
                    PercentAbsent = percentAbsent,
                    PercentLate = percentLate,
                    ReportDate = DateTime.Now
                });
            }

            return summaries;
        }


        public async Task<List<DepartmentSummaryForCeoDto>> GetDepartmentSummariesForCeoAsync(DateTime startDate, DateTime endDate)
        {

            var employees = await _unitOfWork.EmployeeRepository.GetAllASync();

            var groupedByDept = employees
                .Where(e => e.Dept_Id != null)
                .GroupBy(e => e.Dept_Id)
                .ToList();

            var summaries = new List<DepartmentSummaryForCeoDto>();

            foreach (var grp in groupedByDept)
            {
                int deptId = grp.Key.Value;
                string deptName = grp.First().Dept?.dept_name ?? "Unknown";

                int totalEmployees = grp.Count();

                int totalWorkingDays = 0;
                int totalAbsentDays = 0;
                int totalLateDays = 0;

                foreach (var emp in grp)
                {
                    var records = await GetAttendanceRecordsFromSP(emp.Id);
                    var filtered = records
                        .Where(r => r.Date.ToDateTime(TimeOnly.MinValue) >= startDate
                                 && r.Date.ToDateTime(TimeOnly.MinValue) <= endDate)
                        .ToList();

                    totalWorkingDays += filtered.Count;
                    totalAbsentDays += filtered.Count(r => r.IsAbsent);
                    totalLateDays += filtered.Count(r => r.IsLate);
                }

                decimal percentAbsent = totalWorkingDays == 0
                    ? 0
                    : (decimal)totalAbsentDays / totalWorkingDays * 100m;
                decimal percentLate = totalWorkingDays == 0
                    ? 0
                    : (decimal)totalLateDays / totalWorkingDays * 100m;

                summaries.Add(new DepartmentSummaryForCeoDto
                {
                    DepartmentId = deptId,
                    DepartmentName = deptName,
                    TotalEmployees = totalEmployees,
                    PercentAbsent = percentAbsent,
                    PercentLate = percentLate,
                    ReportDate = DateTime.Now
                });
            }

            return summaries;
        }




        private decimal CalculateDayDeduction(DateTime checkIn)
        {
            var baseDate = DateTime.Today;
            var onTime = baseDate.AddHours(8).AddMinutes(30);
            var arrivalTime = baseDate.Add(checkIn.TimeOfDay);

            var quarterLate = baseDate.AddHours(9);
            var halfLate = baseDate.AddHours(9).AddMinutes(30);

            if (arrivalTime <= onTime)
                return 0m;
            else if (arrivalTime <= quarterLate)
                return 0.25m;
            else if (arrivalTime <= halfLate)
                return 0.5m;
            else
                return 1m;
        }

        private void ApplyMonthlyAbsenceRules(List<AttendanceRecordDto> records)
        {
            var groupedByMonth = records
                .Where(r => r.IsAbsent)
                .GroupBy(r => r.Date.ToString("yyyy-MM"));

            foreach (var monthGroup in groupedByMonth)
            {
                var totalHours = monthGroup
                    .Where(r => r.CheckIn.HasValue && r.CheckOut.HasValue)
                    .Sum(r => (r.CheckOut.Value - r.CheckIn.Value).TotalHours);

                var absenceCount = monthGroup.Count();

                decimal deduction = (totalHours <= 4 && absenceCount <= 2) ? 0m : 1m;

                foreach (var record in monthGroup)
                    record.DayDeduction = deduction;
            }
        }

        private void ApplyYearlyAbsenceRules(
            List<AttendanceRecordDto> records,
            dynamic employee,
            out decimal totalDeduction)
        {
            var currentYear = DateTime.Today.Year;
            var resetDate = new DateTime(currentYear, 3, 1);
            var absenceStart = DateTime.Today >= resetDate
                ? resetDate
                : new DateTime(currentYear - 1, 3, 1);

            var yearlyAbsences = records
                .Where(r => r.IsAbsent && r.Date.ToDateTime(TimeOnly.MinValue) >= absenceStart)
                .ToList();

            decimal usedAbsenceDays = yearlyAbsences.Sum(r => r.DayDeduction);
            decimal baseAllowedDays = employee.Is_Manager ? 35m : 21m;
            decimal carriedOverDays = 0m;
            decimal totalAllowedDays = baseAllowedDays + carriedOverDays;

            decimal excessDays = Math.Max(0, usedAbsenceDays - totalAllowedDays);

            totalDeduction = records.Sum(r => r.DayDeduction);
        }

        private async Task<List<AttendanceRecordDto>> GetAttendanceRecordsFromSP(int employeeId)
        {
            var connection = _context.Database.GetDbConnection();
            var records = new List<AttendanceRecordDto>();

            await using (var command = connection.CreateCommand())
            {
                command.CommandText = "GetEmployeeAttendanceReport";
                command.CommandType = CommandType.StoredProcedure;

                var param = new SqlParameter("@EmployeeId", SqlDbType.Int)
                {
                    Value = employeeId
                };

                command.Parameters.Add(param);

                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    records.Add(new AttendanceRecordDto
                    {
                        Date = DateOnly.FromDateTime(reader.GetDateTime(0)),
                        CheckIn = reader.IsDBNull(1) ? null : reader.GetDateTime(1),
                        CheckOut = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                        IsLate = reader.GetBoolean(3),
                        IsAbsent = reader.GetBoolean(4),
                        Status = reader.GetString(5),
                        DayDeduction = 0m
                    });
                }
            }

            return records;
        }
    }
}