using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class FeatureRole
    {
        public int FeatureRoleId { get; set; }
        public int RoleId { get; set; }
        public int FeatureId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Feature Feature { get; set; }
        public virtual Role Role { get; set; }
    }
}
