using BussinessLogic.Models._SubCompanyDto;
using BussinessLogic.ServicesAbstraction;
using DataAccess.Data._UnitOfWork;
using DataAccess.Data.DbContext;

public class Sub_CompanyService : ISub_CompanyService
{
    private readonly IUnitOfWork _unitOfWork;

    public Sub_CompanyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SubCompanyDetailsDto>> GetAllAsync()
    {
        var subs = await _unitOfWork.SubCompanyRepository.GetAllASync();
        return subs
            .Where(e => !e.IsDeleted)
            .Select(s => new SubCompanyDetailsDto
            {
                Company_Id = s.Id,
                Company_Name = s.Sub_Name,
                Company_Address = s.Address,
                Company_Phone = s.Sub_Phone,
                CEO_Id = s.CEO_id
            });
    }

    public async Task<SubCompanyDetailsDto?> GetByIdAsync(int id)
    {
        var sub = await _unitOfWork.SubCompanyRepository.GetByIdAsync(id);
        if (sub == null) return null;

        return new SubCompanyDetailsDto
        {
            Company_Id = sub.Id,
            Company_Name = sub.Sub_Name,
            Company_Address = sub.Address,
            Company_Phone = sub.Sub_Phone,
            CEO_Id = sub.CEO_id
        };
    }

    public async Task<int> AddAsync(CreatedSubCompanyDto dto)
    {
        var entity = new Sub_Company
        {
            Sub_Name = dto.Company_Name,
            Address = dto.Company_Address,
            Sub_Phone = dto.Company_Phone,
            CEO_id = dto.CEO_Id
        };

        await _unitOfWork.SubCompanyRepository.AddAsync(entity);
        _unitOfWork.Complete();
        return entity.Id; 
    }

    public async Task<bool> UpdateAsync(int id, UpdatedSubCompanyDto dto)
    {
        var sub = await _unitOfWork.SubCompanyRepository.GetByIdAsync(id);
        if (sub == null) return false;

        sub.Sub_Name = dto.Company_Name;
        sub.Address = dto.Company_Address;
        sub.Sub_Phone = dto.Company_Phone;
        sub.CEO_id = dto.CEO_Id;

        await _unitOfWork.SubCompanyRepository.UpdateAsync(sub);
        return _unitOfWork.Complete() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sub = await _unitOfWork.SubCompanyRepository.GetByIdAsync(id);
        if (sub == null) return false;

        await _unitOfWork.SubCompanyRepository.DeleteAsync(sub);
        return _unitOfWork.Complete() > 0;
    }
}
