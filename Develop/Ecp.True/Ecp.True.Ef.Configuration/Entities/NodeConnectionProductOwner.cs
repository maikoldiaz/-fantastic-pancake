using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class NodeConnectionProductOwner
    {
        public int NodeConnectionProductOwnerId { get; set; }
        public int OwnerId { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public int NodeConnectionProductId { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual NodeConnectionProduct NodeConnectionProduct { get; set; }
        public virtual CategoryElement Owner { get; set; }
    }
}
