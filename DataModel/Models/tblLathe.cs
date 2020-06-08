using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblLathe")]
    public class tblLathe
    {
        [Key]
        public int LatheId { get; set; }
        //[Required(ErrorMessage = "Please enter name.")]
        public string LatheName { get; set; }
    }
}
