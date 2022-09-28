using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewInventoryInformation
    {
        public int InventoryTransactionId { get; set; }
        public string SystemName { get; set; }
        public int SystemTypeId { get; set; }
        public string SourceSystem { get; set; }
        public string DestinationSystem { get; set; }
        public string EventType { get; set; }
        public string TankName { get; set; }
        public string InventoryId { get; set; }
        public int? TicketId { get; set; }
        public DateTime InventoryDate { get; set; }
        public int NodeId { get; set; }
        public string NodeName { get; set; }
        public bool NodeSendToSap { get; set; }
        public int? SegmentId { get; set; }
        public string SegmentName { get; set; }
        public bool IsDeleted { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public decimal? ProductVolume { get; set; }
        public string MeasurmentUnit { get; set; }
        public decimal? UncertaintyPercentage { get; set; }
        public int? OwnershipTicketId { get; set; }
        public int? ReasonId { get; set; }
        public bool BlockchainStatus { get; set; }
        public int InventoryProductId { get; set; }
        public string MeasurementUnit { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public string Comment { get; set; }
        public string BatchId { get; set; }
    }
}
