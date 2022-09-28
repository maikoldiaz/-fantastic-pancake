using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class MovementDetailsBeforeCutoff
    {
        public string MovementId { get; set; }
        public DateTime? CalculationDate { get; set; }
        public string NodeName { get; set; }
        public string Operacion { get; set; }
        public string SourceNode { get; set; }
        public string DestinationNode { get; set; }
        public string SourceProduct { get; set; }
        public string DestinationProduct { get; set; }
        public decimal? NetStandardVolume { get; set; }
        public decimal? GrossStandardVolume { get; set; }
        public string MeasurementUnit { get; set; }
        public string Movement { get; set; }
        public decimal? PercentStandardUnCertainty { get; set; }
        public decimal? Uncertainty { get; set; }
        public string SourceProductId { get; set; }
        public string Category { get; set; }
        public string Element { get; set; }
        public long? Rno { get; set; }
    }
}
