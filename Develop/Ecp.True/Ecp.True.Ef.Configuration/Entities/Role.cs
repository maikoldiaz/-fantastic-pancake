using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Role
    {
        public Role()
        {
            FeatureRole = new HashSet<FeatureRole>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<FeatureRole> FeatureRole { get; set; }
    }
}
