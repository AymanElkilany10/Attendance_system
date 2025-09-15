using DataAccess.Data.DbContext;
using DataAccess.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Models._AttendanceDto
{
    public class CreatedAttendanceDto
    {
        public DateOnly Attendance_Date { get; set; }

        public DateTime Check_In { get; set; }

        public DateTime Check_Out { get; set; }

        public bool Is_Late { get; set; }

        public bool Is_Absent { get; set; }

        public string Status { get; set; }

    }
}
