using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OwnershipRuleRefreshHistory
    {
        public int OwnershipRuleRefreshHistoryId { get; set; }
        public bool Status { get; set; }
        public string RequestedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
