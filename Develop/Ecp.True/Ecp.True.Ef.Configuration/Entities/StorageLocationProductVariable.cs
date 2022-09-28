using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class StorageLocationProductVariable
    {
        public int StorageLocationProductVariableId { get; set; }
        public int StorageLocationProductId { get; set; }
        public int VariableTypeId { get; set; }
        public byte[] RowVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual StorageLocationProduct StorageLocationProduct { get; set; }
        public virtual VariableType VariableType { get; set; }
    }
}
