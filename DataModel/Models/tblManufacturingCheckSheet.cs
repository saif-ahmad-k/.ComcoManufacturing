using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblManufacturingCheckSheet")]
    public class tblManufacturingCheckSheet
    {
        [Key]
        public int MCS_id { get; set; }
        //[Required(ErrorMessage = "Please enter name.")]
        public string DrawingNumber { get; set; }
        public string CustomerId { get; set; }
        public string PartId { get; set; }
        public string EmployeeId { get; set; }
        public string DateCreated { get; set; }
        public string OperationNumber { get; set; }
        public string InspectionCharacteristicsDimensions { get; set; }
        public int? CharacteristicsIdNumber { get; set; }
        public string InspectionMethod { get; set; }
        public string InspectionFrequency { get; set; }
        public string RevisionNumber { get; set; }
        public Boolean IsRevised { get; set; }
    }
}
