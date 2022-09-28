using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class AuditStatus
    {
        public AuditStatus()
        {
            PipelineLog = new HashSet<PipelineLog>();
        }

        public int StatusId { get; set; }
        public string Status { get; set; }

        public virtual ICollection<PipelineLog> PipelineLog { get; set; }
    }
}
