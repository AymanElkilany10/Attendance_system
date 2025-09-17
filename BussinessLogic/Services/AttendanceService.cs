using BussinessLogic.Models._AttendanceDto;
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
                command.CommandText = "GetEmployeeAttendanceRecords";
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