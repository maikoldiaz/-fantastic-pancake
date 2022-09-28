using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ReportConfiguration
    {
        public int ReportConfigurationId { get; set; }
        public string ReportName { get; set; }
        public string ReportConfigurationName { get; set; }
        public string ReportConfigurationDescription { get; set; }
        public int ReportConfiguartionValue { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
