using BussinessLogic.Models._DepartmentDto;
using BussinessLogic.ServicesAbstraction;
using BussinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Attendance_system.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("GetAllDepartments")]
        public IActionResult GetAllDepartments()
        {
            var result = _departmentService.GetAllDepartments();
            var response = new ApiResponse<IEnumerable<DepartmentDetailsDto>>()
            {
                Code = 200,
                Status = "Success",
                Message = "Departments retrieved successfully",
                Data = result
            };
            return Ok(response);
        }

        [HttpGet("GetDepartmentById/{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var result = await _departmentService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    Code = 404,
                    Status = "Error",
                    Message = "Department not found",
                    Data = null
                });
            }

            var response = new ApiResponse<DepartmentDetailsDto>()
            {
                Code = 200,
                Status = "Success",
                Message = "Department retrieved successfully",
                Data = result
            };
            return Ok(response);
        }

        [HttpPost("CreateDepartment")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreatedDepartmentDto dto)
        {
            var newDeptId = await _departmentService.CreateDepartment(dto);

            var response = new ApiResponse<int>()
            {
                Code = 201,
                Status = "Success",
                Message = "Department created successfully",
                Data = newDeptId
            };
            return CreatedAtAction(nameof(GetDepartmentById), new { id = newDeptId }, response);
        }

        [HttpPut("UpdateDepartment/{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdatedDepartmentDto model)
        {
            var result = await _departmentService.UpdateDepartment(model, id);

            var response = new ApiResponse<string>()
            {
                Code = result ? 200 : 400,
                Status = result ? "Success" : "Error",
                Message = result ? "Department updated successfully" : "Update failed",
                Data = null
            };

            return Ok(response);
        }

        [HttpDelete("DeleteDepartment/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var result = await _departmentService.DeleteDepartment(id);

            var response = new ApiResponse<string>()
            {
                Code = result ? 200 : 404,
                Status = result ? "Success" : "Error",
                Message = result ? "Department deleted successfully" : "Department not found",
                Data = null
            };

            return Ok(response);
        }
    }
}
