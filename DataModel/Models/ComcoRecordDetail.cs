using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("ComcoRecordDetail")]
    public class ComcoRecordDetail
    {
        [Key]
        public int Id { get; set; }
        public string DrawingNumber { get; set; }
        public Nullable<int> lineNumber { get; set; }
        public string toolNumber { get; set; }
        public string OffSetNumber { get; set; }
        public string ToolDescription { get; set; }
        public string SFM_RPM { get; set; }
        public string FEEDiN_REV { get; set; }
        public string Projection { get; set; }
        public string InsertDrillTap { get; set; }
        public string StickHolderBore { get; set; }
        public string HolderOnTurret { get; set; }
        public string QRN1 { get; set; }
        public string QRN2 { get; set; }
        public string QRN3 { get; set; }
        public int CamcoRecordId { get; set; }
        public string ColletBlade { get; set; }
        public string QRN4 { get; set; }
    }
}
