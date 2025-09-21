using BussinessLogic.Models._EmployeeDto;
using BussinessLogic.ServicesAbstraction;
using DataAccess.Data._UnitOfWork;
using DataAccess.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeDetailsDto> GetAllEmployees()
        {
            var employees = _unitOfWork.EmployeeRepository
                .GetAllAsIQueryable()
                .Where(e => !e.IsDeleted)
                .Include(e => e.Dept)
                .Select(employee => new EmployeeDetailsDto
                {
                    Id = employee.Id,
                    FirstName = employee.Fname,
                    LastName = employee.Lname,
                    Emp_Phone = employee.Emp_Phone,
                    Email = employee.Email,
                    Is_Manager = employee.Is_Manager,
                    Dept_Id = employee.Dept_Id,
                    Department = employee.Dept != null ? employee.Dept.dept_name : null
                }).ToList();

            return employees;
        }   

        public async Task<EmployeeDetailsDto?> GetEmployeeById(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (employee is not null)
               return new EmployeeDetailsDto()
               {
                   Id = employee.Id,
                   FirstName = employee.Fname,
                   LastName = employee.Lname,
                   Emp_Phone = employee.Emp_Phone,
                   Email = employee.Email,
                   Is_Manager = employee.Is_Manager,
                   Dept_Id = employee.Dept_Id,
                   Department = employee.Dept != null ? employee.Dept.dept_name : null
               };
            return null;
        }
        public int CreateEmployee(CreatedEmployeeDto employee)
        {
            var Emp = new Employee()
            {
                Fname = employee.FirstName,
                Lname = employee.LastName,
                Emp_Phone = employee.Emp_Phone,
                Email = employee.Email,
                Is_Manager = employee.Is_Manager,
                UserName = employee.UserName,
                PasswordHash = employee.Password,
                Dept_Id = employee.Dept_Id
            };

            _unitOfWork.EmployeeRepository.AddAsync(Emp);
            return _unitOfWork.Complete();
        }
        
        public int UpdateEmployee(UpdatedEmployeeDto employee, int id)
        {
            var em = _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (em is null || id != em.Id)
                return 0;

            var Emp = new Employee()
            {
                Fname = employee.FirstName,
                Lname = employee.LastName,
                Emp_Phone = employee.Emp_Phone,
                Email = employee.Email,
                Is_Manager = employee.Is_Manager,
                UserName = employee.UserName,
                PasswordHash = employee.Password,
                Dept_Id = employee.Dept_Id
            };

            _unitOfWork.EmployeeRepository.UpdateAsync(Emp);
            return _unitOfWork.Complete();
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _unitOfWork.EmployeeRepository.GetByIdAsync(id).Result;

            if (employee is not null)
                _unitOfWork.EmployeeRepository.DeleteAsync(employee);

            return _unitOfWork.Complete() > 0;
        }
    }
}
