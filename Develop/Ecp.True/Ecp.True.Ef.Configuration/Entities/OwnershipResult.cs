using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OwnershipResult
    {
        public int OwnershipResultId { get; set; }
        public int MessageTypeId { get; set; }
        public int? MovementTransactionId { get; set; }
        public int? InventoryProductId { get; set; }
        public int NodeId { get; set; }
        public string ProductId { get; set; }
        public DateTime ExecutionDate { get; set; }
        public decimal InitialInventory { get; set; }
        public decimal FinalInventory { get; set; }
        public decimal Input { get; set; }
        public decimal Output { get; set; }
        public int OwnerId { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public decimal OwnershipVolume { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual InventoryProduct InventoryProduct { get; set; }
        public virtual MessageType MessageType { get; set; }
        public virtual Movement MovementTransaction { get; set; }
        public virtual Node Node { get; set; }
        public virtual Product Product { get; set; }
    }
}
