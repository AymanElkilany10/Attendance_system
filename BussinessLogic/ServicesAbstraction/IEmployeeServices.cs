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
        int CreateEmployee(CreatedEmployeeDto employee);
        int UpdateEmployee(UpdatedEmployeeDto employee, int id); 
        bool DeleteEmployee(int id);
    }
}
