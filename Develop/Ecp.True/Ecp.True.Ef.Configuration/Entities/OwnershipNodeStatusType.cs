using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OwnershipNodeStatusType
    {
        public OwnershipNodeStatusType()
        {
            OwnershipNode = new HashSet<OwnershipNode>();
        }

        public int OwnershipNodeStatusTypeId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<OwnershipNode> OwnershipNode { get; set; }
    }
}
