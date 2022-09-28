using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Feature
    {
        public Feature()
        {
            FeatureRole = new HashSet<FeatureRole>();
        }

        public int FeatureId { get; set; }
        public int ScenarioId { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Scenario Scenario { get; set; }
        public virtual ICollection<FeatureRole> FeatureRole { get; set; }
    }
}
