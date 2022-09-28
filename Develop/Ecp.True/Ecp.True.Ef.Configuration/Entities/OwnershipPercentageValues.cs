using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OwnershipPercentageValues
    {
        public int OwnershipPercentageValuesId { get; set; }
        public DateTime OperationalDate { get; set; }
        public string TransferPoint { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public string SourceSystem { get; set; }
        public DateTime LoadDate { get; set; }
        public Guid? ExecutionId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
