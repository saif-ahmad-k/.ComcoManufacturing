using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblCustomer")]
    public class tblCustomer
    {
        [Key]
        public int CustomerID { get; set; }
        //[Required(ErrorMessage = "Please enter name.")]
        public string CustomerName { get; set; }
        public string CustomerAbbreviation { get; set; }
    }
}
