using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Algorithm
    {
        public Algorithm()
        {
            NodeConnection = new HashSet<NodeConnection>();
            OwnershipAnalytics = new HashSet<OwnershipAnalytics>();
        }

        public int AlgorithmId { get; set; }
        public string ModelName { get; set; }
        public int? PeriodsToForecast { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<NodeConnection> NodeConnection { get; set; }
        public virtual ICollection<OwnershipAnalytics> OwnershipAnalytics { get; set; }
    }
}
