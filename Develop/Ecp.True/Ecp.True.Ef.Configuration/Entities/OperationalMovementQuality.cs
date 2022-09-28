using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OperationalMovementQuality
    {
        public int Rno { get; set; }
        public string MovementId { get; set; }
        public DateTime CalculationDate { get; set; }
        public string MovementTypeName { get; set; }
        public string SourceNode { get; set; }
        public string DestinationNode { get; set; }
        public string SourceProduct { get; set; }
        public string DestinationProduct { get; set; }
        public decimal? NetStandardVolume { get; set; }
        public decimal? GrossStandardVolume { get; set; }
        public string MeasurementUnit { get; set; }
        public string SystemName { get; set; }
        public string Movement { get; set; }
        public decimal? PercentStandardUnCertainty { get; set; }
        public decimal? Uncertainty { get; set; }
        public string AttributeValue { get; set; }
        public string ValueAttributeUnit { get; set; }
        public string AttributeDescription { get; set; }
        public string ProductId { get; set; }
        public string InputCategory { get; set; }
        public string InputElementName { get; set; }
        public string InputNodeName { get; set; }
        public DateTime InputStartDate { get; set; }
        public DateTime InputEndDate { get; set; }
        public string ExecutionId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
