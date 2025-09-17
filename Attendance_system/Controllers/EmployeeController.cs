using BussinessLogic.Models._EmployeeDto;
using BussinessLogic.ServicesAbstraction;
using DataAccess.Data._UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


        [HttpGet]
        public ActionResult<IEnumerable<EmployeeDetailsDto>> GetAllEmployees()
        {
            var employees = _employeeServices.GetAllEmployees();

            return Ok(employees);
        }

         
    }
}
