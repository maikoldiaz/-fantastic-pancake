using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OwnershipAnalytics
    {
        public int OwnershipAnalyticsId { get; set; }
        public int TicketId { get; set; }
        public int AlgorithmId { get; set; }
        public string MovementId { get; set; }
        public string MovementTypeId { get; set; }
        public int SourceNodeId { get; set; }
        public int SourceNodeTypeId { get; set; }
        public int DestinationNodeId { get; set; }
        public int DestinationNodeTypeId { get; set; }
        public string SourceProductId { get; set; }
        public string SourceProductTypeId { get; set; }
        public int? OwnerId { get; set; }
        public decimal? OwnershipVolume { get; set; }
        public decimal? OwnershipPercentage { get; set; }
        public DateTime ExecutionDate { get; set; }
        public int MovementTransactionId { get; set; }
        public string MeasurementUnit { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual Algorithm Algorithm { get; set; }
        public virtual Node DestinationNode { get; set; }
        public virtual CategoryElement DestinationNodeType { get; set; }
        public virtual Movement MovementTransaction { get; set; }
        public virtual CategoryElement Owner { get; set; }
        public virtual Node SourceNode { get; set; }
        public virtual CategoryElement SourceNodeType { get; set; }
        public virtual Product SourceProduct { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
