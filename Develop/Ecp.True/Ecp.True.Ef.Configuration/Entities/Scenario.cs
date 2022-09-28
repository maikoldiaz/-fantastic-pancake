using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Scenario
    {
        public Scenario()
        {
            Feature = new HashSet<Feature>();
        }

        public int ScenarioId { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Feature> Feature { get; set; }
    }
}
