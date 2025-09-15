using DataAccess.Data.Repositories._AttendanceRepository;
using DataAccess.Data.Repositories._DepartmentRepository;
using DataAccess.Data.Repositories._EmployeeRepository;
using DataAccess.Data.Repositories._SubCRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data._UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Attendance_SystemContext _dbContext;

        public UnitOfWork(Attendance_SystemContext context)
        {
            _dbContext = context;
        }
        public IEmployeeRepository EmployeeRepository => new EmployeeRepository(_dbContext);
        public IDepartmentRepository DepartmentRepository => new DepartmentRepository(_dbContext);
        public IAttendanceRepository AttendanceRepository => new AttendanceRepository(_dbContext);
        public ISubCompanyRepository SubCompanyRepository => new SubCompanyRepository(_dbContext);
        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
