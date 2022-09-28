using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class InventoryDetailsWithOwner
    {
        public string Category { get; set; }
        public string Element { get; set; }
        public string NodeName { get; set; }
        public DateTime? CalculationDate { get; set; }
        public string Product { get; set; }
        public string ProductId { get; set; }
        public string OwnerName { get; set; }
        public string InventoryId { get; set; }
        public DateTime InventoryDate { get; set; }
        public string TankName { get; set; }
        public string BatchId { get; set; }
        public decimal? NetVolume { get; set; }
        public string VolumeUnit { get; set; }
        public string SystemName { get; set; }
        public decimal? UncertainityPercentage { get; set; }
        public decimal? Incertidumbre { get; set; }
        public decimal? OwnershipVolume { get; set; }
        public decimal? OwnershipPercentage { get; set; }
        public string AppliedRule { get; set; }
        public long? Rno { get; set; }
    }
}
