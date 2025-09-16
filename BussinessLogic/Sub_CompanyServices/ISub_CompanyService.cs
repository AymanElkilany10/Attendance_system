using BussinessLogic.Models._SubCompanyDto;
using DataAccess.Data.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Sub_CompanyServices
{
    public interface ISub_CompanyService
    {
        Task<IEnumerable<Sub_Company>> GetAllAsync();
        Task<Sub_Company?> GetByIdAsync(int id);
        Task AddAsync(Sub_Company sub_Company);
        Task<bool> UpdateAsync(int id,UpdatedSubCompanyDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
