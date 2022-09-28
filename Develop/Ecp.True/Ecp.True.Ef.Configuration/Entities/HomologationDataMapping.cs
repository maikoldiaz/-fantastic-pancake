using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class HomologationDataMapping
    {
        public int HomologationDataMappingId { get; set; }
        public string SourceValue { get; set; }
        public string DestinationValue { get; set; }
        public int HomologationGroupId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual HomologationGroup HomologationGroup { get; set; }
    }
}
