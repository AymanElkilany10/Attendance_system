
using BussinessLogic.Services;
using BussinessLogic.ServicesAbstraction;
using DataAccess.Data._UnitOfWork;
using DataAccess.Data.Repositories._AttendanceRepository;
using DataAccess.Data.Repositories._DepartmentRepository;
using DataAccess.Data.Repositories._EmployeeRepository;
using DataAccess.Data.Repositories._GenericRepository;
using DataAccess.Data.Repositories._SubCRepository;
using Microsoft.EntityFrameworkCore;

namespace Attendance_system
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<Attendance_SystemContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefualtConnection"));
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            builder.Services.AddScoped<ISubCompanyRepository, SubCompanyRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<ISub_CompanyService, Sub_CompanyService>();
            builder.Services.AddScoped<IEmployeeServices, EmployeeServices>();
            builder.Services.AddScoped<IAttendanceService, AttendanceService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
