using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Models._ManagerSummary
{
    public class ManagerAttendanceSummaryDto
    {
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }

        public int TotalEmployees { get; set; }
        public decimal PercentAbsent { get; set; }
        public decimal PercentLate { get; set; }

        public DateTime ReportDate { get; set; }
    }
}
