using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    [Table("tblOperation")]
    public class tblOperation
    {
        [Key]
        public int Operation_Id { get; set; }
        //[Required(ErrorMessage = "Please enter name.")]
        public string RM_DESC { get; set; }
        public int PartId { get; set; }
        public string RM_WKCTR { get; set; }
        public Boolean? RM_SWITCH { get; set; }
        public Boolean? VERIFIED_CYCLETIME { get; set; }
        public string RM_CT { get; set; }
        public string CT_MINUTES { get; set; }
        public string RM_OP { get; set; }
        [NotMapped]
        [Display(Name = "Part #")]
        public string Part
        {
            get
            {
                string Qnum = "";
                using (BaseDataContext db = new BaseDataContext())
                {
                    if (PartId != null)
                    {
                        Qnum = db.tParts.Find(PartId).Part;
                    }
                }
                return Qnum;
            }
        }
        [NotMapped]
        [Display(Name = "Description")]
        public string Description
        {
            get
            {
                string Qnum = "";
                using (BaseDataContext db = new BaseDataContext())
                {
                    if (RM_DESC != null && RM_OP != null)
                    {
                        Qnum = RM_DESC + "_" + RM_OP;
                    }
                    else if(RM_DESC != null && RM_OP == null)
                    {
                        Qnum = RM_DESC + "";
                    }
                    else if (RM_DESC == null && RM_OP != null)
                    {
                        Qnum = "" + RM_OP;
                    }
                    else
                    {
                        Qnum = "";
                    }
                }
                return Qnum;
            }
        }
    }
}
