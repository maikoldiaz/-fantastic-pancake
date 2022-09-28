using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class PipelineLog
    {
        public PipelineLog()
        {
            ErrorLog = new HashSet<ErrorLog>();
        }

        public int PipelineId { get; set; }
        public string PipelineRunId { get; set; }
        public string PipelineName { get; set; }
        public int PipelineStatusId { get; set; }
        public DateTime PipelineStartTime { get; set; }
        public DateTime? PipelineEndTime { get; set; }

        public virtual AuditStatus PipelineStatus { get; set; }
        public virtual ICollection<ErrorLog> ErrorLog { get; set; }
    }
}
