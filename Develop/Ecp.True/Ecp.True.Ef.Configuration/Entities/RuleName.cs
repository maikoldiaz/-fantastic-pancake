using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class RuleName
    {
        public RuleName()
        {
            Rule = new HashSet<Rule>();
        }

        public int RuleNameId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Rule> Rule { get; set; }
    }
}
