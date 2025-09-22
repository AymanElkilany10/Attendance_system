using BussinessLogic.Models._DepartmentDto;
using BussinessLogic.ServicesAbstraction;
using DataAccess.Data._UnitOfWork;
using DataAccess.Data.DbContext;
using DataAccess.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<DepartmentDetailsDto> GetAllDepartments()
        {
            var departments = _unitOfWork.DepartmentRepository
                .GetAllAsIQueryable()
                .Where(e => !e.IsDeleted)
                .Select(dept => new DepartmentDetailsDto
                {
                    Id = dept.Id,
                    dept_name = dept.dept_name,
                    subCompany_id = dept.sub_id,
                    Manager_Id = dept.Manager_Id,
                    subCompanyName = dept.sub != null ? dept.sub.Sub_Name : null,
                    ManagerName = dept.Manager != null ? $"{dept.Manager.Fname} {dept.Manager.Lname}" : null
                }).ToList();

            return departments;
        }

        public async Task<DepartmentDetailsDto?> GetByIdAsync(int id)
        {
            var dept = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (dept is not null)
            {
                return new DepartmentDetailsDto
                {
                    Id = dept.Id,
                    dept_name = dept.dept_name,
                    subCompany_id = dept.sub_id,
                    Manager_Id = dept.Manager_Id,
                    subCompanyName = dept.sub != null ? dept.sub.Sub_Name : null,
                    ManagerName = dept.Manager != null ? $"{dept.Manager.Fname} {dept.Manager.Lname}" : null
                };
            }
            return null;
        }

        public async Task<int> CreateDepartment(CreatedDepartmentDto dto)
        {
            var Dept = new Department
            {
                dept_name = dto.dept_name,
                sub_id = dto.sub_id,
                Manager_Id = dto.Manager_Id
            };

            await _unitOfWork.DepartmentRepository.AddAsync(Dept);
            return _unitOfWork.Complete();
    
        }

        public async Task<bool> UpdateDepartment(UpdatedDepartmentDto dto, int id)
        {
            var dept = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (dept is null || id != dept.Id)
                return false;


            dept.dept_name = dto.dept_name;
            dept.sub_id = dto.sub_id;
            dept.Manager_Id = dto.Manager_Id;
            
            await _unitOfWork.DepartmentRepository.UpdateAsync(dept);
           return  _unitOfWork.Complete() > 0;
        }

        public async Task<bool> DeleteDepartment(int id)
        {
            var dept = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (dept is not null)
            {
                await _unitOfWork.DepartmentRepository.DeleteAsync(dept);
                return _unitOfWork.Complete() > 0;
            }

            return false;
        }
    }
   
}
