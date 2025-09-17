using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Models._AttendanceDto
{
    public class AttendanceRecordDto
    {
        public DateOnly Date { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public bool IsLate { get; set; }
        public bool IsAbsent { get; set; }
        public string Status { get; set; }
        public decimal DayDeduction { get; set; }
    }
}
