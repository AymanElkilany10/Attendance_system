using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Models._DepartmentDto
{
    public class CreatedDepartmentDto
    {
        public string dept_name { get; set; }
        public int sub_id { get; set; }
        public int? Manager_Id { get; set; }
    }
}
