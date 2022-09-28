using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Ownership
    {
        public int OwnershipId { get; set; }
        public int MessageTypeId { get; set; }
        public int TicketId { get; set; }
        public int? MovementTransactionId { get; set; }
        public int? InventoryProductId { get; set; }
        public int OwnerId { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public decimal OwnershipVolume { get; set; }
        public string AppliedRule { get; set; }
        public string RuleVersion { get; set; }
        public DateTime ExecutionDate { get; set; }
        public bool? BlockchainStatus { get; set; }
        public Guid? BlockchainMovementTransactionId { get; set; }
        public Guid? BlockchainInventoryProductTransactionId { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? BlockchainOwnershipId { get; set; }
        public Guid? PreviousBlockchainOwnershipId { get; set; }
        public string TransactionHash { get; set; }
        public string BlockNumber { get; set; }
        public int RetryCount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual InventoryProduct InventoryProduct { get; set; }
        public virtual MessageType MessageType { get; set; }
        public virtual Movement MovementTransaction { get; set; }
        public virtual CategoryElement Owner { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
