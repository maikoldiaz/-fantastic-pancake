using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Attribute
    {
        public int Id { get; set; }
        public string AttributeId { get; set; }
        public string AttributeValue { get; set; }
        public string ValueAttributeUnit { get; set; }
        public string AttributeDescription { get; set; }
        public int? InventoryProductId { get; set; }
        public int? MovementTransactionId { get; set; }
        public string AttributeType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual InventoryProduct InventoryProduct { get; set; }
        public virtual Movement MovementTransaction { get; set; }
    }
}
