using DataAccess.Data.Entities;
using DataAccess.Data.Repositories._GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.Repositories._AttendanceRepository
{
    public class AttendanceRepository : GenericRepository<Attendance> ,IAttendanceRepository
    {
        private readonly Attendance_SystemContext _context;

        public AttendanceRepository(Attendance_SystemContext context) : base(context)
        {
            _context = context;
        }
    }
}
