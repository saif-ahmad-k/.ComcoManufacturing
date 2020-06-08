using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblProduct")]
    public class tblProduct
    {
        [Key]
        public int Product_ID { get; set; }
        //[Required(ErrorMessage = "Please enter name.")]
        public Nullable<int> CategoryId { get; set; }
        public string ProductName { get; set; }
        public Decimal? Cost { get; set; }
        public string QRN { get; set; }
        public byte[] ProductImage { get; set; }
    }
}
