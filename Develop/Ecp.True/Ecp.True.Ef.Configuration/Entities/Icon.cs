using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Icon
    {
        public Icon()
        {
            CategoryElement = new HashSet<CategoryElement>();
        }

        public int IconId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<CategoryElement> CategoryElement { get; set; }
    }
}
