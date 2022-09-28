using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class AuditStatus1
    {
        public AuditStatus1()
        {
            PipelineLog1 = new HashSet<PipelineLog1>();
        }

        public int StatusId { get; set; }
        public string Status { get; set; }

        public virtual ICollection<PipelineLog1> PipelineLog1 { get; set; }
    }
}
