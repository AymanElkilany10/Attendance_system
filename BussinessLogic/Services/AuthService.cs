using BussinessLogic.Models._EmployeeDto;
using BussinessLogic.ServicesAbstraction;
using BussinessLogic.ViewModels;
using DataAccess.Data._UnitOfWork;
using DataAccess.Data.DbContext;
using DataAccess.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration config)
    {
        _unitOfWork = unitOfWork;
        _config = config;
    }

    public async Task<ApiResponse<string>> Register(CreatedEmployeeDto dto)
    {
        var exists = await _unitOfWork.EmployeeRepository
            .AnyAsync(e => e.UserName == dto.UserName || e.Email == dto.Email);

        if (exists)
        {
            return new ApiResponse<string>
            {
                Code = 400,
                Status = "Error",
                Message = "Username or Email already exists",
                Data = null
            };
        }

        var allowedRoles = new[] { "CEO", "DepartmentManager", "LineManager" };
        if (!allowedRoles.Contains(dto.Role))
        {
            return new ApiResponse<string>
            {
                Code = 400,
                Status = "Error",
                Message = "Invalid role. Allowed roles: CEO, DepartmentManager, LineManager",
                Data = null
            };
        }

        if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
        {
            return new ApiResponse<string>
            {
                Code = 400,
                Status = "Error",
                Message = "Password must be at least 6 characters long",
                Data = null
            };
        }

        var employee = new Employee
        {
            Fname = dto.FirstName,
            Lname = dto.LastName,
            Email = dto.Email,
            UserName = dto.UserName,
            Dept_Id = dto.Dept_Id,
            Line_Manager_Id = dto.Line_Manager_Id,
            Role = dto.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _unitOfWork.EmployeeRepository.AddAsync(employee);
              _unitOfWork.Complete();

        return new ApiResponse<string>
        {
            Code = 201,
            Status = "Success",
            Message = "Registration successful",
            Data = null
        };
    }

    public async Task<ApiResponse<string>> Login(string username, string password)
    {
        var user = (await _unitOfWork.EmployeeRepository.GetAllASync())
                        .FirstOrDefault(e => e.UserName == username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return new ApiResponse<string>
            {
                Code = 401,
                Status = "Error",
                Message = "Invalid credentials",
                Data = null
            };
        }

        var token = GenerateJwtToken(user);

        return new ApiResponse<string>
        {
            Code = 200,
            Status = "Success",
            Message = "Login successful",
            Data = token
        };
    }

    private string GenerateJwtToken(Employee user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("EmployeeId", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
