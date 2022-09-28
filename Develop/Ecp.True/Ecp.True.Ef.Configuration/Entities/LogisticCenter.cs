using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class LogisticCenter
    {
        public LogisticCenter()
        {
            Node = new HashSet<Node>();
            StorageLocation = new HashSet<StorageLocation>();
        }

        public string LogisticCenterId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<Node> Node { get; set; }
        public virtual ICollection<StorageLocation> StorageLocation { get; set; }
    }
}
