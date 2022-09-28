using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class HomologationObject
    {
        public int HomologationObjectId { get; set; }
        public int HomologationObjectTypeId { get; set; }
        public bool? IsRequiredMapping { get; set; }
        public int HomologationGroupId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual HomologationGroup HomologationGroup { get; set; }
        public virtual HomologationObjectType HomologationObjectType { get; set; }
    }
}
