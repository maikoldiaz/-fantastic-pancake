using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ErrorLog1
    {
        public int ErrorId { get; set; }
        public int PipelineId { get; set; }
        public string ErrorMsg { get; set; }
        public DateTime ErrorDate { get; set; }

        public virtual PipelineLog1 Pipeline { get; set; }
    }
}
