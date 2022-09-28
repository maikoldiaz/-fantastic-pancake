using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class StorageLocation
    {
        public StorageLocation()
        {
            NodeStorageLocation = new HashSet<NodeStorageLocation>();
            StorageLocationProductMapping = new HashSet<StorageLocationProductMapping>();
        }

        public string StorageLocationId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public string LogisticCenterId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual LogisticCenter LogisticCenter { get; set; }
        public virtual ICollection<NodeStorageLocation> NodeStorageLocation { get; set; }
        public virtual ICollection<StorageLocationProductMapping> StorageLocationProductMapping { get; set; }
    }
}
