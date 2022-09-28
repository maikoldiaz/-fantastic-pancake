using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class FileRegistrationTransaction
    {
        public FileRegistrationTransaction()
        {
            Inventory = new HashSet<Inventory>();
            InventoryProduct = new HashSet<InventoryProduct>();
            Movement = new HashSet<Movement>();
        }

        public int FileRegistrationTransactionId { get; set; }
        public int FileRegistrationId { get; set; }
        public string BlobPath { get; set; }
        public string SessionId { get; set; }
        public int StatusTypeId { get; set; }
        public string RecordId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual FileRegistration FileRegistration { get; set; }
        public virtual StatusType StatusType { get; set; }
        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<InventoryProduct> InventoryProduct { get; set; }
        public virtual ICollection<Movement> Movement { get; set; }
    }
}
