using BussinessLogic.Models._EmployeeDto;
using BussinessLogic.ServicesAbstraction;
using BussinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(CreatedEmployeeDto dto)
    {
        var response = await _authService.Register(dto);
        return StatusCode(response.Code, response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        var response = await _authService.Login(username, password);
        return StatusCode(response.Code, response);
    }
}
