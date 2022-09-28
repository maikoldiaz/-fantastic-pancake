using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class AttributeDetailsWithOwner
    {
        public string MovementId { get; set; }
        public DateTime? OperationalDate { get; set; }
        public string Operacion { get; set; }
        public string SourceNodeName { get; set; }
        public string DestinationNodeName { get; set; }
        public string SourceProductName { get; set; }
        public string DestinationProductName { get; set; }
        public decimal? NetStandardVolume { get; set; }
        public decimal? GrossStandardVolume { get; set; }
        public string MeasurementUnit { get; set; }
        public string SystemName { get; set; }
        public int? SourceMovementId { get; set; }
        public int? Order { get; set; }
        public int? Position { get; set; }
        public string OwnerName { get; set; }
        public decimal? OwnershipVolume { get; set; }
        public string AttributeValue { get; set; }
        public string ValueAttributeUnit { get; set; }
        public string AttributeDescription { get; set; }
        public DateTime OwnershipProcessDate { get; set; }
        public decimal? Uncertainty { get; set; }
        public string SourceProductId { get; set; }
        public string Category { get; set; }
        public string Element { get; set; }
        public string NodeName { get; set; }
        public DateTime? CalculationDate { get; set; }
        public long? Rno { get; set; }
    }
}
