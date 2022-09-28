using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Reversal
    {
        public int ReversalId { get; set; }
        public int SourceMovementTypeId { get; set; }
        public int ReversedMovementTypeId { get; set; }
        public int SourceNodeId { get; set; }
        public int DestinationNodeId { get; set; }
        public int SourceProductId { get; set; }
        public int DestinationProductId { get; set; }
        public bool? IsActive { get; set; }
        public byte[] RowVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual OriginType DestinationNode { get; set; }
        public virtual OriginType DestinationProduct { get; set; }
        public virtual CategoryElement ReversedMovementType { get; set; }
        public virtual CategoryElement SourceMovementType { get; set; }
        public virtual OriginType SourceNode { get; set; }
        public virtual OriginType SourceProduct { get; set; }
    }
}
