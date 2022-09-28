using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class StorageLocationProduct
    {
        public StorageLocationProduct()
        {
            StorageLocationProductOwner = new HashSet<StorageLocationProductOwner>();
            StorageLocationProductVariable = new HashSet<StorageLocationProductVariable>();
        }

        public int StorageLocationProductId { get; set; }
        public bool? IsActive { get; set; }
        public string ProductId { get; set; }
        public int NodeStorageLocationId { get; set; }
        public int? OwnershipRuleId { get; set; }
        public decimal? UncertaintyPercentage { get; set; }
        public int? NodeProductRuleId { get; set; }
        public byte[] RowVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual NodeProductRule NodeProductRule { get; set; }
        public virtual NodeStorageLocation NodeStorageLocation { get; set; }
        public virtual CategoryElement OwnershipRule { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<StorageLocationProductOwner> StorageLocationProductOwner { get; set; }
        public virtual ICollection<StorageLocationProductVariable> StorageLocationProductVariable { get; set; }
    }
}
