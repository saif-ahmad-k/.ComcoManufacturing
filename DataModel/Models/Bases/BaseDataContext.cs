using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    public class BaseDataContext:DbContext
    {
        public BaseDataContext() : base("name=BaseDataContext")
        {

        }

        public virtual DbSet<tblParts> tParts { get; set; }
        public virtual DbSet<tblManufacturingCheckSheet> tManufacturingCheckSheets { get; set; }
        public virtual DbSet<tblLathe> tLathes { get; set; }
        public virtual DbSet<tblEmployee> tEmployees { get; set; }
        public virtual DbSet<tblCustomer> tCustomers { get; set; }
        public virtual DbSet<tblCamcoRecord> tCamcoRecords { get; set; }
        public virtual DbSet<ComcoRecordDetail> tComcoRecordDetails { get; set; }
        public virtual DbSet<tblCategory> tCategories { get; set; }
        public virtual DbSet<tblProduct> tProducts { get; set; }
        public virtual DbSet<tblChuckCollet> tChuckCollets { get; set; }
        public virtual DbSet<tblTurrentType> tTurrentTypes { get; set; }
        public virtual DbSet<tblMachineType> tMachineTypes { get; set; }
        public virtual DbSet<tblOperation> tOperations { get; set; }
        public virtual DbSet<tTurretHolder> tTurretHolders { get; set; }
        public virtual DbSet<tStickHolder> tStickHolders { get; set; }
        public virtual DbSet<tTurretInsert> tTurretInserts { get; set; }
        public virtual DbSet<tblHolderType> tblHolderTypes { get; set; }
    }
}
