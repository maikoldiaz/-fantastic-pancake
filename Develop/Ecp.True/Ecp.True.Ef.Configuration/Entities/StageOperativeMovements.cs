using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class StageOperativeMovements
    {
        public DateTime OperationalDate { get; set; }
        public string DestinationNode { get; set; }
        public string DestinationNodeType { get; set; }
        public string MovementType { get; set; }
        public string SourceNode { get; set; }
        public string SourceNodeType { get; set; }
        public string SourceProduct { get; set; }
        public string SourceProductType { get; set; }
        public string TransferPoint { get; set; }
        public string FieldWaterProduction { get; set; }
        public string SourceField { get; set; }
        public string RelatedSourceField { get; set; }
        public decimal NetStandardVolume { get; set; }
        public string SourceSystem { get; set; }
        public DateTime LoadDate { get; set; }
        public Guid? ExecutionId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
