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
        BaseDataContext db = new BaseDataContext();
        [Key]
        public int Product_ID { get; set; }
        //[Required(ErrorMessage = "Please enter name.")]
        public Nullable<int> CategoryId { get; set; }
        public string ProductName { get; set; }
        public Decimal? Cost { get; set; }
        public string QRN { get; set; }
        public byte[] ProductImage { get; set; }
        public int? ParentId { get; set; }
        public bool? IsParent { get; set; }
        public int? HolderTypeId { get; set; }
        public bool? IsParentInsert { get; set; }
        private string pName;
        [NotMapped]
        public string Parent
        {
            get
            {
                if (CategoryId != null)
                {
                    return db.tCategories.Find(CategoryId).Name;
                }
                else
                {
                    return "";
                }

            }
            set
            {
                if (CategoryId != null)
                {
                    if (value == db.tCategories.Find(CategoryId).Name)
                        pName = value;
                }
                else
                {
                    pName = "";
                }
            }
        }
    }
}
