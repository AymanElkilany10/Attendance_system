using BussinessLogic.ServicesAbstraction;
using BussinessLogic.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Attendance_system.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendaceReportController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendaceReportController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }


        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetEmployeeAttendanceReport(int employeeId)
        {
            var report = await _attendanceService.GetEmployeeAttendanceReport(employeeId);
            if (report == null)
            {
                var notFoundResponse = new ApiResponse<object>
                {
                    Code = 404,
                    Status = "Not Found",
                    Message = "Employee not found or no attendance records available.",
                    Data = null
                };
                return NotFound(notFoundResponse);
            }
            var successResponse = new ApiResponse<object>
            {
                Code = 200,
                Status = "Success",
                Message = "Attendance report retrieved successfully.",
                Data = report
            };
            return Ok(successResponse);
        }



        [HttpGet("manager-report/{managerId}")]
        public async Task<IActionResult> GetManagerAttendanceReport(int managerId)
        {
            var reports = await _attendanceService.GetReportsForManager(managerId);

            if (reports == null || !reports.Any())
            {
                var notFoundResponse = new ApiResponse<object>
                {
                    Code = 404,
                    Status = "Not Found",
                    Message = "No attendance reports found for this manager.",
                    Data = null
                };

                return NotFound(notFoundResponse);
            }

            var successResponse = new ApiResponse<object>
            {
                Code = 200,
                Status = "Success",
                Message = "Attendance reports retrieved successfully.",
                Data = reports
            };

            return Ok(successResponse);
        }

    }
}
