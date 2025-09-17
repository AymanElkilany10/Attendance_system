using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Models._AttendanceDto
{
    public class EmployeeAttendanceReportDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public int TotalDays { get; set; }
        public int DaysPresent { get; set; }
        public int DaysAbsent { get; set; }
        public int DaysLate { get; set; }


        public decimal TotalDeduction { get; set; }
        public List<AttendanceRecordDto> Records { get; set; }
    }
}
