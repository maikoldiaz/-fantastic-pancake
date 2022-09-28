using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ControlScript
    {
        public Guid Id { get; set; }
        public bool Status { get; set; }
        public string DeploymentType { get; set; }
        public DateTime ExecutedDate { get; set; }
    }
}
