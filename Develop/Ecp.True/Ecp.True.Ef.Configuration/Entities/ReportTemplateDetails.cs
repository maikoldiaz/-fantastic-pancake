using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ReportTemplateDetails
    {
        public string ReportIdentifier { get; set; }
        public string Area { get; set; }
        public string InformationResponsible { get; set; }
        public string Frequency { get; set; }
        public string InformationSource { get; set; }
        public string Datamart { get; set; }
        public string Version { get; set; }
        public string UpdateDate { get; set; }
        public string ChangeResponsible { get; set; }
    }
}
