using DataAccess.Data.DbContext;
using DataAccess.Data.Repositories._GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.Repositories._EmployeeRepository
{
    public class EmployeeRepository : GenericRepository<Employee>,IEmployeeRepository
    {
        public EmployeeRepository(Attendance_SystemContext dbContext) : base(dbContext)
        {

        }
    }
}
