using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Rule
    {
        public Rule()
        {
            NodeConnectionProduct = new HashSet<NodeConnectionProduct>();
        }

        public int RuleId { get; set; }
        public int RuleNameId { get; set; }
        public int RuleTypeId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual RuleName RuleName { get; set; }
        public virtual RuleType RuleType { get; set; }
        public virtual ICollection<NodeConnectionProduct> NodeConnectionProduct { get; set; }
    }
}
