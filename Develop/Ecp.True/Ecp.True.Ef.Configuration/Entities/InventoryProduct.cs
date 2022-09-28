using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class InventoryProduct
    {
        public InventoryProduct()
        {
            Attribute = new HashSet<Attribute>();
            Owner = new HashSet<Owner>();
            Ownership = new HashSet<Ownership>();
            OwnershipNodeError = new HashSet<OwnershipNodeError>();
            OwnershipResult = new HashSet<OwnershipResult>();
        }

        public int InventoryProductId { get; set; }
        public string ProductId { get; set; }
        public string ProductType { get; set; }
        public decimal? ProductVolume { get; set; }
        public decimal? GrossStandardQuantity { get; set; }
        public string MeasurementUnit { get; set; }
        public decimal? UncertaintyPercentage { get; set; }
        public int? OwnershipTicketId { get; set; }
        public int? ReasonId { get; set; }
        public string Comment { get; set; }
        public bool BlockchainStatus { get; set; }
        public Guid? BlockchainInventoryProductTransactionId { get; set; }
        public Guid? PreviousBlockchainInventoryProductTransactionId { get; set; }
        public string TransactionHash { get; set; }
        public string BlockNumber { get; set; }
        public int RetryCount { get; set; }
        public int SystemTypeId { get; set; }
        public string SystemName { get; set; }
        public string SourceSystem { get; set; }
        public string DestinationSystem { get; set; }
        public string EventType { get; set; }
        public string TankName { get; set; }
        public string InventoryId { get; set; }
        public int? TicketId { get; set; }
        public DateTime InventoryDate { get; set; }
        public int NodeId { get; set; }
        public int? SegmentId { get; set; }
        public string Observations { get; set; }
        public int? ScenarioId { get; set; }
        public bool IsDeleted { get; set; }
        public int? FileRegistrationTransactionId { get; set; }
        public string Operator { get; set; }
        public string BatchId { get; set; }
        public string InventoryProductUniqueId { get; set; }
        public int? SystemId { get; set; }
        public string Version { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual FileRegistrationTransaction FileRegistrationTransaction { get; set; }
        public virtual Node Node { get; set; }
        public virtual Ticket OwnershipTicket { get; set; }
        public virtual Product Product { get; set; }
        public virtual CategoryElement Reason { get; set; }
        public virtual ScenarioType Scenario { get; set; }
        public virtual CategoryElement Segment { get; set; }
        public virtual CategoryElement System { get; set; }
        public virtual SystemType SystemType { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual ICollection<Attribute> Attribute { get; set; }
        public virtual ICollection<Owner> Owner { get; set; }
        public virtual ICollection<Ownership> Ownership { get; set; }
        public virtual ICollection<OwnershipNodeError> OwnershipNodeError { get; set; }
        public virtual ICollection<OwnershipResult> OwnershipResult { get; set; }
    }
}
