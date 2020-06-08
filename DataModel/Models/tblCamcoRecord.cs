using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblCamcoRecord")]
    public class tblCamcoRecord
    {
        [Key]
        public int Id { get; set; }
        //[Required(ErrorMessage = "Please enter name.")]
        public Nullable<double> CustomerId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<int> LatheId { get; set; }
        public Nullable<int> PartId { get; set; }
        public string DrawingNumber { get; set; }
        public string OperationNumber { get; set; }
        public string CNCProgramNumber { get; set; }
        public string Fixturing { get; set; }
        public string Cycletime { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ITTPartShift { get; set; }
        public Nullable<int> Chuck_Collet_Id { get; set; }
        public Nullable<int> MachineId { get; set; }
        public Nullable<int> TurrentTypeId { get; set; }
        public string ChuckPresure { get; set; }
        public string ColletQRN { get; set; }
        public int? OperationId { get; set; }
        public byte[] CamcoRecordImage { get; set; }
    }
}
