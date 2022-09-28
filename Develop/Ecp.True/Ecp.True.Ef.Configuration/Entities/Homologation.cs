using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Homologation
    {
        public Homologation()
        {
            HomologationGroup = new HashSet<HomologationGroup>();
        }

        public int HomologationId { get; set; }
        public int SourceSystemId { get; set; }
        public int DestinationSystemId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual SystemType DestinationSystem { get; set; }
        public virtual SystemType SourceSystem { get; set; }
        public virtual ICollection<HomologationGroup> HomologationGroup { get; set; }
    }
}
