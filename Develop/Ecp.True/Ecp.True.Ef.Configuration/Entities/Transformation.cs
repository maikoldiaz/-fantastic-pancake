using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Transformation
    {
        public int TransformationId { get; set; }
        public int MessageTypeId { get; set; }
        public int OriginSourceNodeId { get; set; }
        public int? OriginDestinationNodeId { get; set; }
        public string OriginSourceProductId { get; set; }
        public string OriginDestinationProductId { get; set; }
        public int OriginMeasurementId { get; set; }
        public int DestinationSourceNodeId { get; set; }
        public int? DestinationDestinationNodeId { get; set; }
        public string DestinationSourceProductId { get; set; }
        public string DestinationDestinationProductId { get; set; }
        public int DestinationMeasurementId { get; set; }
        public bool? IsDeleted { get; set; }
        public byte[] RowVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual Node DestinationDestinationNode { get; set; }
        public virtual Product DestinationDestinationProduct { get; set; }
        public virtual CategoryElement DestinationMeasurement { get; set; }
        public virtual Node DestinationSourceNode { get; set; }
        public virtual Product DestinationSourceProduct { get; set; }
        public virtual MessageType MessageType { get; set; }
        public virtual Node OriginDestinationNode { get; set; }
        public virtual Product OriginDestinationProduct { get; set; }
        public virtual CategoryElement OriginMeasurement { get; set; }
        public virtual Node OriginSourceNode { get; set; }
        public virtual Product OriginSourceProduct { get; set; }
    }
}
