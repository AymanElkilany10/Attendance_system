using BussinessLogic.Models._EmployeeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.ServicesAbstraction
{
    public interface IEmployeeServices
    {
        IEnumerable<EmployeeDetailsDto> GetAllEmployees();
        Task<EmployeeDetailsDto?> GetEmployeeById(int id);
        Task<int> CreateEmployee(CreatedEmployeeDto employee);
        Task<int> UpdateEmployee(UpdatedEmployeeDto employee, int id); 
        Task<bool> DeleteEmployee(int id);
    }
}
