using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class NodeOwnershipRule
    {
        public NodeOwnershipRule()
        {
            Node = new HashSet<Node>();
        }

        public int RuleId { get; set; }
        public string RuleName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<Node> Node { get; set; }
    }
}
