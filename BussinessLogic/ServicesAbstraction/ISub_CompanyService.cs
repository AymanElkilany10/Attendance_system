using BussinessLogic.Models._SubCompanyDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BussinessLogic.ServicesAbstraction
{
    public interface ISub_CompanyService
    {
        Task<IEnumerable<SubCompanyDetailsDto>> GetAllAsync();
        Task<SubCompanyDetailsDto?> GetByIdAsync(int id);
        Task<int> AddAsync(CreatedSubCompanyDto dto);
        Task<bool> UpdateAsync(int id, UpdatedSubCompanyDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
