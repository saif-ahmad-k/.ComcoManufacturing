using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblMachineType")]
    public class tblMachineType
    {
        [Key]
        public int Machine_Id { get; set; }
        public string MachineName { get; set; }
    }
}
