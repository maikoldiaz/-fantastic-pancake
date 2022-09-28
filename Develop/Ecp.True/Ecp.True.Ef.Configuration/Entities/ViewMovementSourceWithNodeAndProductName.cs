using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewMovementSourceWithNodeAndProductName
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
        public string SourceNode { get; set; }
        public string SourceProduct { get; set; }
    }
}
