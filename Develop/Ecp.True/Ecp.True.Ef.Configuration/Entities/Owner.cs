using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Owner
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public decimal OwnershipValue { get; set; }
        public string OwnershipValueUnit { get; set; }
        public int? InventoryProductId { get; set; }
        public int? MovementTransactionId { get; set; }
        public bool? BlockchainStatus { get; set; }
        public Guid? BlockchainMovementTransactionId { get; set; }
        public Guid? BlockchainInventoryProductTransactionId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual InventoryProduct InventoryProduct { get; set; }
        public virtual Movement MovementTransaction { get; set; }
    }
}
