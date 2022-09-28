using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewStorageLocationProductWithProductName
    {
        public int StorageLocationProductId { get; set; }
        public bool IsActive { get; set; }
        public string ProductId { get; set; }
        public int NodeStorageLocationId { get; set; }
        public int? OwnershipRuleId { get; set; }
        public decimal? UncertaintyPercentage { get; set; }
        public int? NodeProductRuleId { get; set; }
        public byte[] RowVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string ProductName { get; set; }
    }
}
