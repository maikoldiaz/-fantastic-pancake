using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Category
    {
        public Category()
        {
            CategoryElement = new HashSet<CategoryElement>();
            HomologationGroup = new HashSet<HomologationGroup>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public bool IsGrouper { get; set; }
        public bool? IsReadOnly { get; set; }
        public bool IsHomologation { get; set; }
        public byte[] RowVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<CategoryElement> CategoryElement { get; set; }
        public virtual ICollection<HomologationGroup> HomologationGroup { get; set; }
    }
}
