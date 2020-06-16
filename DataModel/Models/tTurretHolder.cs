using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblTurretHolder")]
    public class tTurretHolder
    {
        [Key]
        public int TurretHolder_Id { get; set; }
        public string TurretHolderName { get; set; }
        public string TurretHolderQRN { get; set; }
        public byte[] TurretHolderImage { get; set; }
        public int TurretTypeId { get; set; }
    }
}
