using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class HomologationGroup
    {
        public HomologationGroup()
        {
            HomologationDataMapping = new HashSet<HomologationDataMapping>();
            HomologationObject = new HashSet<HomologationObject>();
        }

        public int HomologationGroupId { get; set; }
        public int GroupTypeId { get; set; }
        public int HomologationId { get; set; }
        public byte[] RowVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual Category GroupType { get; set; }
        public virtual Homologation Homologation { get; set; }
        public virtual ICollection<HomologationDataMapping> HomologationDataMapping { get; set; }
        public virtual ICollection<HomologationObject> HomologationObject { get; set; }
    }
}
