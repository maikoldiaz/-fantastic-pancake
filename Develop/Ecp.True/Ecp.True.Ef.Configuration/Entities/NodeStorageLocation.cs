using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class NodeStorageLocation
    {
        public NodeStorageLocation()
        {
            MovementDestination = new HashSet<MovementDestination>();
            MovementSource = new HashSet<MovementSource>();
            StorageLocationProduct = new HashSet<StorageLocationProduct>();
        }

        public int NodeStorageLocationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StorageLocationTypeId { get; set; }
        public bool? IsActive { get; set; }
        public int NodeId { get; set; }
        public bool SendToSap { get; set; }
        public string StorageLocationId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual Node Node { get; set; }
        public virtual StorageLocation StorageLocation { get; set; }
        public virtual CategoryElement StorageLocationType { get; set; }
        public virtual ICollection<MovementDestination> MovementDestination { get; set; }
        public virtual ICollection<MovementSource> MovementSource { get; set; }
        public virtual ICollection<StorageLocationProduct> StorageLocationProduct { get; set; }
    }
}
