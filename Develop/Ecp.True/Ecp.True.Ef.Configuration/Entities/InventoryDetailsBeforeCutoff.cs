using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class InventoryDetailsBeforeCutoff
    {
        public string InventoryId { get; set; }
        public DateTime? CalculationDate { get; set; }
        public string NodeName { get; set; }
        public string TankName { get; set; }
        public string Product { get; set; }
        public decimal? NetVolume { get; set; }
        public string MeasurementUnit { get; set; }
        public decimal? UncertaintyPercentage { get; set; }
        public string ProductId { get; set; }
        public string Category { get; set; }
        public string Element { get; set; }
        public long? Rno { get; set; }
    }
}
