using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblEmployees")]
    public class tblEmployee
    {
        [Key]
        public int EmployeeId { get; set; }
        //[Required(ErrorMessage = "Please enter name.")]
        public string EmployeeName { get; set; }
    }
}
