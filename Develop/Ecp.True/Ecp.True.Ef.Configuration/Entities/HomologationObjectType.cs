using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class HomologationObjectType
    {
        public HomologationObjectType()
        {
            HomologationObject = new HashSet<HomologationObject>();
        }

        public int HomologationObjectTypeId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<HomologationObject> HomologationObject { get; set; }
    }
}
