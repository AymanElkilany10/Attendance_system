using DataAccess.Data.Entities;
using DataAccess.Data.Repositories._GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.Repositories._AttendanceRepository
{
    public interface IAttendanceRepository : IGenericRepository<Attendance>
    {
    }
}
