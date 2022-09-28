using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OriginType
    {
        public OriginType()
        {
            ReversalDestinationNode = new HashSet<Reversal>();
            ReversalDestinationProduct = new HashSet<Reversal>();
            ReversalSourceNode = new HashSet<Reversal>();
            ReversalSourceProduct = new HashSet<Reversal>();
        }

        public int OriginTypeId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Reversal> ReversalDestinationNode { get; set; }
        public virtual ICollection<Reversal> ReversalDestinationProduct { get; set; }
        public virtual ICollection<Reversal> ReversalSourceNode { get; set; }
        public virtual ICollection<Reversal> ReversalSourceProduct { get; set; }
    }
}
