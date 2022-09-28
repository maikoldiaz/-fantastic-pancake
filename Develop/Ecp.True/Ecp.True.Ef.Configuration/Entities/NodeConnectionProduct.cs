using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class NodeConnectionProduct
    {
        public NodeConnectionProduct()
        {
            NodeConnectionProductOwner = new HashSet<NodeConnectionProductOwner>();
        }

        public int NodeConnectionProductId { get; set; }
        public int NodeConnectionId { get; set; }
        public string ProductId { get; set; }
        public decimal? UncertaintyPercentage { get; set; }
        public bool IsDeleted { get; set; }
        public int? Priority { get; set; }
        public int? NodeConnectionProductRuleId { get; set; }
        public byte[] RowVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual NodeConnection NodeConnection { get; set; }
        public virtual NodeConnectionProductRule NodeConnectionProductRule { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<NodeConnectionProductOwner> NodeConnectionProductOwner { get; set; }
    }
}
