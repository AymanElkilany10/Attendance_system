using DataAccess.Data.DbContext;
using DataAccess.Data.Entities;
using DataAccess.Data.Repositories._GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Data.Repositories._EmployeeRepository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(Attendance_SystemContext dbContext) : base(dbContext)
        {
        }

        public override async Task<IEnumerable<Employee>> GetAllASync()
        {
            return await _context.Set<Employee>()
                         .Include(e => e.Dept)
                         .Where(e => !e.IsDeleted)
                         .ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<Employee, bool>> predicate)
        {
            return await _context.Employees.AnyAsync(predicate);
        }
    }
}
