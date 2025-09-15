using DataAccess.Data.DbContext;
using DataAccess.Data.Repositories._GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.Repositories._SubCRepository
{
    public class SubCompanyRepository : GenericRepository<Sub_Company> ,ISubCompanyRepository
    {
        public SubCompanyRepository(Attendance_SystemContext context) : base(context)
        {
            
        }
    }
}
