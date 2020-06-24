using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblHolderType")]
    public class tblHolderType
    {
        public enum HolderTypesEnum
        {
            TurretHolder = 1,
            StickHolder = 2,
            Insert = 3,
            ColletBlade = 4
        }
        [Key]
        public int Holder_Id { get; set; }
        public string HolderName { get; set; }
    }
}
