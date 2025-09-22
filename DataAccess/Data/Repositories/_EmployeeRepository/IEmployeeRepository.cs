using DataAccess.Data.DbContext;
using DataAccess.Data.Repositories._GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.Repositories._EmployeeRepository
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<bool> AnyAsync(Expression<Func<Employee, bool>> predicate);
    }
}
