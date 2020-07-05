using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblCategory")]
    public class tblCategory
    {
        BaseDataContext db = new BaseDataContext();
        [Key]
        public int Category_ID { get; set; }
        //[Required(ErrorMessage = "Please enter name.")]
        public Nullable<int> ParentId { get; set; }
        public string Name { get; set; }
        public bool? IsParent { get; set; }
        public Nullable<int> MachineId { get; set; }
        public byte[] CategoryImage { get; set; }
        private string pName;
        [NotMapped]
        public string Parent
        {
            get
            {
                if (ParentId != null)
                {
                    return db.tCategories.Find(ParentId).Name;
                }
                else
                {
                    return "";
                }
                
            }
            set
            {
                if (ParentId != null)
                {
                    if (value == db.tCategories.Find(ParentId).Name)
                        pName = value;
                }
                else
                {
                    pName = "";
                }
            }
        }
        [NotMapped]
        public bool isSelected { get; set; }
    }
}
