using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class SystemType
    {
        public SystemType()
        {
            FileRegistration = new HashSet<FileRegistration>();
            HomologationDestinationSystem = new HashSet<Homologation>();
            HomologationSourceSystem = new HashSet<Homologation>();
            Inventory = new HashSet<Inventory>();
            InventoryProduct = new HashSet<InventoryProduct>();
            Movement = new HashSet<Movement>();
            PendingTransaction = new HashSet<PendingTransaction>();
        }

        public int SystemTypeId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<FileRegistration> FileRegistration { get; set; }
        public virtual ICollection<Homologation> HomologationDestinationSystem { get; set; }
        public virtual ICollection<Homologation> HomologationSourceSystem { get; set; }
        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<InventoryProduct> InventoryProduct { get; set; }
        public virtual ICollection<Movement> Movement { get; set; }
        public virtual ICollection<PendingTransaction> PendingTransaction { get; set; }
    }
}
