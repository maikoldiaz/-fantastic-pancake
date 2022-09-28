using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class RuleType
    {
        public RuleType()
        {
            Rule = new HashSet<Rule>();
        }

        public int RuleTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Rule> Rule { get; set; }
    }
}
