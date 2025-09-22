using BussinessLogic.Models._EmployeeDto;
using BussinessLogic.ServicesAbstraction;
using BussinessLogic.ViewModels;
using DataAccess.Data._UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Attendance_system.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices _employeeServices;

        public EmployeeController(IEmployeeServices employeeServices)
        {
            _employeeServices = employeeServices;
        }

        [Authorize(Roles = "CEO,DepartmentManager,LineManager")]
        [HttpGet]
        public ActionResult GetAllEmployees()
        {
            var employees = _employeeServices.GetAllEmployees();
            var response = new ApiResponse<IEnumerable<EmployeeDetailsDto>> 
            {
                Code = 200,
                Status = "Success",
                Message = "Employees retrieved successfully",
                Data = employees
            };

            return Ok(response);
        }

        [Authorize(Roles = "CEO,DepartmentManager,LineManager")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetEmployeeById(int id)
        {
            var employee = await _employeeServices.GetEmployeeById(id);
            if (employee == null)
            {
                var notFoundResponse = new ApiResponse<EmployeeDetailsDto>
                {
                    Code = 404,
                    Status = "Error",
                    Message = "Employee not found",
                    Data = null
                };
                return NotFound(notFoundResponse);
            }
            var response = new ApiResponse<EmployeeDetailsDto>
            {
                Code = 200,
                Status = "Success",
                Message = "Employee retrieved successfully",
                Data = employee
            };
            return Ok(response);
        }

        [Authorize(Roles = "CEO,DepartmentManager,LineManager")]
        [HttpPost]
        public async Task<ActionResult> CreateEmployee([FromBody] CreatedEmployeeDto employee)
        {
            var createdEmployeeId = await _employeeServices.CreateEmployee(employee);

            var response = new ApiResponse<int>
            {
                Code = 201,
                Status = "Success",
                Message = "Employee created successfully",
                Data = createdEmployeeId
            };
            return await Task.FromResult<ActionResult>(CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployeeId }, response));
        }

        [Authorize(Roles = "CEO,DepartmentManager,LineManager")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEmployee([FromBody] UpdatedEmployeeDto employee, int id)
        {
            var updatedEmployeeId = await _employeeServices.UpdateEmployee(employee, id);
            if (updatedEmployeeId == 0)
            {
                var notFoundResponse = new ApiResponse<int>
                {
                    Code = 404,
                    Status = "Error",
                    Message = "Employee not found",
                    Data = 0
                };
                return NotFound(notFoundResponse);
            }
            var response = new ApiResponse<int>
            {
                Code = 200,
                Status = "Success",
                Message = "Employee updated successfully",
                Data = updatedEmployeeId
            };
            return Ok(response);
        }

        [Authorize(Roles = "CEO,DepartmentManager,LineManager")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var isDeleted = await _employeeServices.DeleteEmployee(id);
            if (!isDeleted)
            {
                var notFoundResponse = new ApiResponse<bool>
                {
                    Code = 404,
                    Status = "Error",
                    Message = "Employee not found",
                    Data = false
                };
                return NotFound(notFoundResponse);
            }
            var response = new ApiResponse<bool>
            {
                Code = 200,
                Status = "Success",
                Message = "Employee deleted successfully",
                Data = true
            };
            return Ok(response);
        }
    }
}
