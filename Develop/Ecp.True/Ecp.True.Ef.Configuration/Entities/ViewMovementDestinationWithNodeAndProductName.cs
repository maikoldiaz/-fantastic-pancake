using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewMovementDestinationWithNodeAndProductName
    {
        public int MovementDestinationId { get; set; }
        public int MovementTransactionId { get; set; }
        public int? DestinationNodeId { get; set; }
        public int? DestinationStorageLocationId { get; set; }
        public string DestinationProductId { get; set; }
        public string DestinationProductTypeId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DestinationNode { get; set; }
        public string DestinationProduct { get; set; }
    }
}
