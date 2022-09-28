using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ControlType
    {
        public ControlType()
        {
            OwnershipCalculationResult = new HashSet<OwnershipCalculationResult>();
        }

        public int ControlTypeId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<OwnershipCalculationResult> OwnershipCalculationResult { get; set; }
    }
}
