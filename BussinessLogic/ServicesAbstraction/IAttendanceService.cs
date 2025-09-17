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
    }
}
