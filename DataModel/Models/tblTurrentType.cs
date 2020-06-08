using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblTurrentType")]
    public class tblTurrentType
    {
        [Key]
        public int TurrentType_Id { get; set; }
        public string TurrentTypeName { get; set; }
        public int MachineTypeId { get; set; }
    }
}
