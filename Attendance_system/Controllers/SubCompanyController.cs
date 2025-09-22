using BussinessLogic.Models._SubCompanyDto;
using BussinessLogic.ServicesAbstraction;
using BussinessLogic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance_system.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCompanyController : ControllerBase
    {
        private readonly ISub_CompanyService _subCompanyService;

        public SubCompanyController(ISub_CompanyService subCompanyService)
        {
            _subCompanyService = subCompanyService;
        }

        [Authorize(Roles = "CEO")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _subCompanyService.GetAllAsync();

            var response = new ApiResponse<IEnumerable<SubCompanyDetailsDto>>()
            {
                Code = result.Any() ? 200 : 404,
                Status = result.Any() ? "Success" : "Not Found",
                Message = result.Any() ? "SubCompanies retrieved successfully" : "No subcompanies found",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        [Authorize(Roles = "CEO")]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _subCompanyService.GetByIdAsync(id);

            var response = new ApiResponse<SubCompanyDetailsDto?>()
            {
                Code = result is not null ? 200 : 404,
                Status = result is not null ? "Success" : "Error",
                Message = result is not null ? "SubCompany retrieved successfully" : "SubCompany not found",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        [Authorize(Roles = "CEO")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreatedSubCompanyDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>()
                {
                    Code = 400,
                    Status = "Error",
                    Message = "Invalid subcompany data",
                    Data = null
                });
            }

            var createdId = await _subCompanyService.AddAsync(dto);

            var response = new ApiResponse<int>()
            {
                Code = 201,
                Status = "Success",
                Message = "SubCompany created successfully",
                Data = createdId
            };

            return StatusCode(201, response);
        }

        [Authorize(Roles = "CEO")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatedSubCompanyDto dto)
        {
            var result = await _subCompanyService.UpdateAsync(id, dto);

            var response = new ApiResponse<string>()
            {
                Code = result ? 200 : 400,
                Status = result ? "Success" : "Error",
                Message = result ? "SubCompany updated successfully" : "Update failed",
                Data = null
            };

            return StatusCode(response.Code, response);
        }

        [Authorize(Roles = "CEO")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _subCompanyService.DeleteAsync(id);

            var response = new ApiResponse<string>()
            {
                Code = result ? 200 : 404,
                Status = result ? "Success" : "Error",
                Message = result ? "SubCompany deleted successfully" : "SubCompany not found",
                Data = null
            };

            return StatusCode(response.Code, response);
        }
    }
}
