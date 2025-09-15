using DataAccess.Data.DbContext;
using DataAccess.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Models._EmployeeDto
{
    public class CreatedEmployeeDto
    {

        [MaxLength(50, ErrorMessage = "Max length of Name is 50 Chars")]
        [MinLength(3, ErrorMessage = "Min length of Name is 3 Chars")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "Max length of Name is 50 Chars")]
        [MinLength(3, ErrorMessage = "Min length of Name is 3 Chars")]
        public string LastName { get; set; }

        [Phone]
        public int? Emp_Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool? Is_Manager { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int Dept_Id { get; set; }

    }
}