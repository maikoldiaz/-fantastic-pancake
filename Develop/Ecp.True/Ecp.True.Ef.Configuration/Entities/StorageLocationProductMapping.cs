using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class StorageLocationProductMapping
    {
        public int StorageLocationProductMappingId { get; set; }
        public string StorageLocationId { get; set; }
        public string ProductId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Product Product { get; set; }
        public virtual StorageLocation StorageLocation { get; set; }
    }
}
