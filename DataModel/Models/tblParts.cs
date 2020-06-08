using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblParts")]
    public class tblParts
    {
        [Key]
        public int PartId { get; set; }
        //[Required(ErrorMessage = "Please enter name.")]
        public string Part { get; set; }
        public string Material { get; set; }
        public int? CustomerId { get; set; }
    }
}
