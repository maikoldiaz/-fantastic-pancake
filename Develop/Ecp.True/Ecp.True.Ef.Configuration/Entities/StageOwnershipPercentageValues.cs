using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class StageOwnershipPercentageValues
    {
        public DateTime OperationalDate { get; set; }
        public string TransferPoint { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public string SourceSystem { get; set; }
        public DateTime LoadDate { get; set; }
        public Guid? ExecutionId { get; set; }
    }
}
