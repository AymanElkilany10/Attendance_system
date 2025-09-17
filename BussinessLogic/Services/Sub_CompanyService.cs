using BussinessLogic.Models._SubCompanyDto;
using BussinessLogic.ServicesAbstraction;
using DataAccess.Data._UnitOfWork;
using DataAccess.Data.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Services
{
    public class Sub_CompanyService : ISub_CompanyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public Sub_CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        async Task ISub_CompanyService.AddAsync(Sub_Company sub_Company)
        {
            await _unitOfWork.SubCompanyRepository.AddAsync(sub_Company);
            _unitOfWork.Complete();
        }
        async Task<bool> ISub_CompanyService.DeleteAsync(int id)
        {
            var sub = await _unitOfWork.SubCompanyRepository.GetByIdAsync(id);
            if (sub == null) return false;
            await _unitOfWork.SubCompanyRepository.DeleteAsync(sub);
            return true;
        }

        async Task<IEnumerable<Sub_Company>> ISub_CompanyService.GetAllAsync() => await _unitOfWork.SubCompanyRepository.GetAllAsync();

        async Task<Sub_Company?> ISub_CompanyService.GetByIdAsync(int id) => await _unitOfWork.SubCompanyRepository.GetByIdAsync(id);

        async Task<bool> ISub_CompanyService.UpdateAsync(int id, UpdatedSubCompanyDto dto)
        {
            var sub = await _unitOfWork.SubCompanyRepository.GetByIdAsync(id);
            if (sub == null) return false;
            if (!string.IsNullOrEmpty(dto.Company_Name))
                sub.Sub_Name = dto.Company_Name;
            return await _unitOfWork.SubCompanyRepository.UpdateAsync(sub);
        }
    }
}
