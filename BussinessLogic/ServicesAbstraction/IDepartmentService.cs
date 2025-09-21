using BussinessLogic.Models._DepartmentDto;
using DataAccess.Data.DbContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BussinessLogic.ServicesAbstraction
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentDetailsDto> GetAllDepartments();

        Task<DepartmentDetailsDto?> GetByIdAsync(int id);

        Task<int> CreateDepartment(CreatedDepartmentDto dto);

        Task<bool> UpdateDepartment(UpdatedDepartmentDto dto, int id);

        Task<bool> DeleteDepartment(int id);
    }
}
