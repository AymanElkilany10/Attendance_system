using DataAccess.Data.DbContext;
using DataAccess.Data.Repositories._GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.Repositories._DepartmentRepository
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(Attendance_SystemContext context) : base(context)
        {
            
        }

        Task IDepartmentRepository.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
