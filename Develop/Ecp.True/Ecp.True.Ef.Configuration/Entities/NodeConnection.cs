using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class NodeConnection
    {
        public NodeConnection()
        {
            NodeConnectionProduct = new HashSet<NodeConnectionProduct>();
        }

        public int NodeConnectionId { get; set; }
        public int SourceNodeId { get; set; }
        public int DestinationNodeId { get; set; }
        public string Description { get; set; }
        public decimal? ControlLimit { get; set; }
        public int? AlgorithmId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsTransfer { get; set; }
        public byte[] RowVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual Algorithm Algorithm { get; set; }
        public virtual Node DestinationNode { get; set; }
        public virtual Node SourceNode { get; set; }
        public virtual ICollection<NodeConnectionProduct> NodeConnectionProduct { get; set; }
    }
}
