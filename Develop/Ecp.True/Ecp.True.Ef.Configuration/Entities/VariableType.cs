using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class VariableType
    {
        public VariableType()
        {
            Movement = new HashSet<Movement>();
            StorageLocationProductVariable = new HashSet<StorageLocationProductVariable>();
        }

        public int VariableTypeId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string FicoName { get; set; }
        public bool IsConfigurable { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Movement> Movement { get; set; }
        public virtual ICollection<StorageLocationProductVariable> StorageLocationProductVariable { get; set; }
    }
}
