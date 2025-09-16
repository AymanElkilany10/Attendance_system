using BussinessLogic.Models._DepartmentDto;
using DataAccess.Data.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.DepartmentServices
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllDepartmentsASync();

       Task<Department?> GetByIdAsync(int id);

        Task CreateDepartmentASync(Department department);

        Task<bool> UpdateDepartmentAsync(int id,UpdatedDepartmentDto Dto);

        Task<bool> DeleteDepartmentAsync(int id);

    }
}
