using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblStickHolder")]
    public class tStickHolder
    {
        [Key]
        public int StickHolder_Id { get; set; }
        public string StickHolderName { get; set; }
        public string StickHolderQRN { get; set; }
        public byte[] StickHolderImage { get; set; }
        public int TurretTypeId { get; set; }
        public int? TurretHolderId { get; set; }
    }
}
