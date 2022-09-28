using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OwnershipNodeError
    {
        public int OwnershipNodeErrorId { get; set; }
        public int OwnershipNodeId { get; set; }
        public int? InventoryProductId { get; set; }
        public int? MovementTransactionId { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? ExecutionDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual InventoryProduct InventoryProduct { get; set; }
        public virtual Movement MovementTransaction { get; set; }
        public virtual OwnershipNode OwnershipNode { get; set; }
    }
}
