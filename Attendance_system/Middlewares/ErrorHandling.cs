using BussinessLogic.ViewModels;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;  

public class ErrorHandling
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandling> _logger;
    private readonly IWebHostEnvironment _env;

    public ErrorHandling(RequestDelegate next, ILogger<ErrorHandling> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ApiResponse<string>
            {
                Code = context.Response.StatusCode,
                Status = "Error",
                Message = ex.Message,
                Data = null
            };

            if (_env.IsDevelopment())
            {
                response.Data = ex.StackTrace;
            }

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
