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

    public async Task<ApiResponse<bool>> Register(CreatedEmployeeDto dto)
    {
        var exists = await _unitOfWork.EmployeeRepository.AnyAsync(e => e.UserName == dto.UserName);
        if (exists)
            return new ApiResponse<bool> { Code = 400, Status = "error", Message = "User already exists" };

        var employee = new Employee
        {
            UserName = dto.UserName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role
        };

        await _unitOfWork.EmployeeRepository.AddAsync(employee);
        _unitOfWork.Complete();

        return new ApiResponse<bool>
        {
            Code = 200,
            Status = "success",
            Message = "User registered successfully",
            Data = true
        };
    }


    public async Task<ApiResponse<string>> Login(string username, string password)
    {
        var user = (await _unitOfWork.EmployeeRepository.GetAllASync())
                        .FirstOrDefault(e => e.UserName == username);

        if (user == null)
            return new ApiResponse<string> { Code = 401, Status = "error", Message = "Invalid credentials" };

        if (string.IsNullOrWhiteSpace(user.PasswordHash) || !user.PasswordHash.StartsWith("$2"))
        {
            return new ApiResponse<string>
            {
                Code = 500,
                Status = "error",
                Message = "Stored password hash is invalid or not using bcrypt format."
            };
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return new ApiResponse<string> { Code = 401, Status = "error", Message = "Invalid credentials" };

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
