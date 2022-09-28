using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewMovementInformation
    {
        public string MovementId { get; set; }
        public string MovementTypeId { get; set; }
        public string MovementTypeName { get; set; }
        public int MovementTransactionId { get; set; }
        public int MessageTypeId { get; set; }
        public int SystemTypeId { get; set; }
        public string SourceSystem { get; set; }
        public string SystemName { get; set; }
        public string EventType { get; set; }
        public int? TicketId { get; set; }
        public int? OwnershipTicketId { get; set; }
        public DateTime OperationalDate { get; set; }
        public int? SegmentId { get; set; }
        public string MeasurementUnit { get; set; }
        public decimal? GrossStandardVolume { get; set; }
        public decimal NetStandardVolume { get; set; }
        public decimal? UncertaintyPercentage { get; set; }
        public int? VariableTypeId { get; set; }
        public int? ReasonId { get; set; }
        public string Comment { get; set; }
        public int? ContractId { get; set; }
        public int? SourceNodeId { get; set; }
        public string SourceNodeName { get; set; }
        public int? DestinationNodeId { get; set; }
        public string DestinationNodeName { get; set; }
        public string SourceProductId { get; set; }
        public string SourceProductName { get; set; }
        public string SourceProductTypeId { get; set; }
        public string DestinationProductId { get; set; }
        public string DestinationProductName { get; set; }
        public string DestinationProductTypeId { get; set; }
        public int? SourceStorageLocationId { get; set; }
        public int? DestinationStorageLocationId { get; set; }
    }
}
