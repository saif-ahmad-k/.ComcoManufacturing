using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblOperation")]
    public class tblOperation
    {
        [Key]
        public int Operation_Id { get; set; }
        //[Required(ErrorMessage = "Please enter name.")]
        public string OperationName { get; set; }
        public int PartId { get; set; }
    }
}
