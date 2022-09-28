using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OwnershipCalculationResult
    {
        public int OwnershipCalculationResultId { get; set; }
        public int OwnershipCalculationId { get; set; }
        public int? ControlTypeId { get; set; }
        public int? OwnerId { get; set; }
        public decimal? OwnershipPercentage { get; set; }
        public decimal? OwnershipVolume { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ControlType ControlType { get; set; }
        public virtual CategoryElement Owner { get; set; }
        public virtual OwnershipCalculation OwnershipCalculation { get; set; }
    }
}
