using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblTurretInsert")]
    public class tTurretInsert
    {
        [Key]
        public int Insert_Id { get; set; }
        public string InsertName { get; set; }
        public string InsertQRN { get; set; }
        public byte[] InsertImage { get; set; }
        public int TurretTypeId { get; set; }
        public int? StickHolderId { get; set; }
    }
}
