
using DataAccess.Data.DbContext;
using DataAccess.Data.Repositories._AttendanceRepository;
using DataAccess.Data.Repositories._DepartmentRepository;
using DataAccess.Data.Repositories._EmployeeRepository;
using DataAccess.Data.Repositories._GenericRepository;
using DataAccess.Data.Repositories._SubCRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data._UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IEmployeeRepository EmployeeRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }
        public IAttendanceRepository AttendanceRepository { get; }
        public ISubCompanyRepository SubCompanyRepository { get; }
        public int Complete();
        public void Dispose();
    }
   
}
