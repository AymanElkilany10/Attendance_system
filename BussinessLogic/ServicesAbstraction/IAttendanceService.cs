using BussinessLogic.Models._AttendanceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.ServicesAbstraction
{
    public interface IAttendanceService
    {
        Task<EmployeeAttendanceReportDto?> GetEmployeeAttendanceReport(int employeeId);
        public  Task<List<EmployeeAttendanceReportDto>> GetReportsForManager(int managerId);
        Task<List<LineManagerTeamSummaryDto>> GetLineManagerSummariesByDepartmentManagerAsync(int departmentManagerId, DateTime startDate, DateTime endDate);
        Task<List<DepartmentSummaryForCeoDto>> GetDepartmentSummariesForCeoAsync(DateTime startDate, DateTime endDate);

    }
}
