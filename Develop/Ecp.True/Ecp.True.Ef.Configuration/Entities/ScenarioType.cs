using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ScenarioType
    {
        public ScenarioType()
        {
            InventoryProduct = new HashSet<InventoryProduct>();
            Movement = new HashSet<Movement>();
        }

        public int ScenarioTypeId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<InventoryProduct> InventoryProduct { get; set; }
        public virtual ICollection<Movement> Movement { get; set; }
    }
}
