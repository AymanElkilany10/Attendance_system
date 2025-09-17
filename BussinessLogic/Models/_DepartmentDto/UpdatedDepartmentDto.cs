using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Models._DepartmentDto
{
    public class UpdatedDepartmentDto
    {
        public string? dept_name { get; set; }
        public int sub_id { get; set; }
        public int Manager_Id { get; set; }
    }
}
