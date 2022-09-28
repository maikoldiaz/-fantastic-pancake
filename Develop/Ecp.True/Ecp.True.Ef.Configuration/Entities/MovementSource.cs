using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class MovementSource
    {
        public int MovementSourceId { get; set; }
        public int MovementTransactionId { get; set; }
        public int? SourceNodeId { get; set; }
        public int? SourceStorageLocationId { get; set; }
        public string SourceProductId { get; set; }
        public string SourceProductTypeId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual Movement MovementTransaction { get; set; }
        public virtual Node SourceNode { get; set; }
        public virtual Product SourceProduct { get; set; }
        public virtual NodeStorageLocation SourceStorageLocation { get; set; }
    }
}
