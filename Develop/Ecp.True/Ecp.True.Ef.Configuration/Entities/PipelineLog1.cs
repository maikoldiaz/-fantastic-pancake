using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class PipelineLog1
    {
        public PipelineLog1()
        {
            ErrorLog1 = new HashSet<ErrorLog1>();
        }

        public int PipelineId { get; set; }
        public string PipelineRunId { get; set; }
        public string PipelineName { get; set; }
        public int PipelineStatusId { get; set; }
        public DateTime PipelineStartTime { get; set; }
        public DateTime? PipelineEndTime { get; set; }

        public virtual AuditStatus1 PipelineStatus { get; set; }
        public virtual ICollection<ErrorLog1> ErrorLog1 { get; set; }
    }
}
