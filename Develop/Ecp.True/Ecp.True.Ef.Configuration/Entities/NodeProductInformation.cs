using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class NodeProductInformation
    {
        public string ProductName { get; set; }
        public string ProductOrder { get; set; }
        public string OwnershipStrategy { get; set; }
        public string OwnerName { get; set; }
        public decimal? OwnershipPercentage { get; set; }
        public decimal? UncertaintyNodeProduct { get; set; }
        public string Category { get; set; }
        public string Element { get; set; }
        public string NodeName { get; set; }
        public int NodeId { get; set; }
        public int? Pi { get; set; }
        public int? Interfase { get; set; }
        public int? Pni { get; set; }
        public int? Tolerancia { get; set; }
        public int? Inventario { get; set; }
    }
}
