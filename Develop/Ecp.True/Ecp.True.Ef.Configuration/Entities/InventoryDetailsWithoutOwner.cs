using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class InventoryDetailsWithoutOwner
    {
        public string InventoryId { get; set; }
        public DateTime? CalculationDate { get; set; }
        public int NodeId { get; set; }
        public string NodeName { get; set; }
        public string TankName { get; set; }
        public string BatchId { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductVolume { get; set; }
        public string MeasurmentUnit { get; set; }
        public string SystemName { get; set; }
        public decimal? UncertainityPercentage { get; set; }
        public decimal? Incertidumbre { get; set; }
        public int? TicketId { get; set; }
        public string ProdutId { get; set; }
        public string Category { get; set; }
        public string Element { get; set; }
        public long? Rno { get; set; }
    }
}
