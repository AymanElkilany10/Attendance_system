using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Models._AttendanceDto
{
    public class LineManagerTeamSummaryDto
    {
        public int LineManagerId { get; set; }
        public string LineManagerName { get; set; }
        public int TotalEmployees { get; set; }
        public decimal PercentAbsent { get; set; }
        public decimal PercentLate { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
