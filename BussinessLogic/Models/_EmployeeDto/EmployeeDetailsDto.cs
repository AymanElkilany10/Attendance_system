using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Models._EmployeeDto
{
    public class EmployeeDetailsDto
    {
        [Required]
        public int Emp_Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Emp_Phone { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
        public bool? Is_Manager { get; set; }
        public int? Line_Manager_Id { get; set; }

        public int? Dept_Id { get; set; }
        public string? Department { get; set; }
    }
}
