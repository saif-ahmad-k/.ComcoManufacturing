using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblChuckCollet")]
    public class tblChuckCollet
    {
        [Key]
        public int Id { get; set; }
        //[Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }
    }
}
