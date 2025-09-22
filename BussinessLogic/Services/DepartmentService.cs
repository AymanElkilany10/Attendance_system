using BussinessLogic.Models._DepartmentDto;
using BussinessLogic.ServicesAbstraction;
using DataAccess.Data._UnitOfWork;
using DataAccess.Data.DbContext;
using DataAccess.Data.Repositories._DepartmentRepository;
using DataAccess.Data.Repositories._GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsASync()
        {
            return await _unitOfWork.DepartmentRepository.GetAllASync();
        }

        public async Task<Department?> GetByIdAsync(int id ) {
            return await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
        }

        public async Task CreateDepartmentASync(Department department) {
            await _unitOfWork.DepartmentRepository.AddAsync(department);
            _unitOfWork.Complete();
        }

        public async Task<bool> UpdateDepartmentAsync(int id,UpdatedDepartmentDto Dto)
        {
         var dept =  await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (dept == null) return false;
            if(!string.IsNullOrEmpty(Dto.dept_name)) 
             dept.dept_name = Dto.dept_name;
            return await _unitOfWork.DepartmentRepository.UpdateAsync(dept);
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var dept = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (dept == null)  return false;
            await _unitOfWork.DepartmentRepository.DeleteAsync(dept);
            return true;
        }

    }
}
