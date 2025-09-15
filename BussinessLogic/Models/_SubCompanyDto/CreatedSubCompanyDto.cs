using DataAccess.Data.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Models._SubCompanyDto
{
    public class CreatedSubCompanyDto
    {
        public string Company_Name { get; set; }

        public string Company_Address { get; set; }

        public int? Company_Phone { get; set; }

        public int CEO_Id { get; set; }
    }
}
