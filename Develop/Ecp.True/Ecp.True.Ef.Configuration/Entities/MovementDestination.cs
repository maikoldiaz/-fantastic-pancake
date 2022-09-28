using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class MovementDestination
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

        public virtual Node DestinationNode { get; set; }
        public virtual Product DestinationProduct { get; set; }
        public virtual NodeStorageLocation DestinationStorageLocation { get; set; }
        public virtual Movement MovementTransaction { get; set; }
    }
}
