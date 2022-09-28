using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class QualityDetailsWithOwner
    {
        public string Category { get; set; }
        public string Element { get; set; }
        public string NodeName { get; set; }
        public DateTime? CalculationDate { get; set; }
        public string Product { get; set; }
        public string ProductId { get; set; }
        public string InventoryId { get; set; }
        public DateTime InventoryDate { get; set; }
        public string TankName { get; set; }
        public string BatchId { get; set; }
        public int NodeId { get; set; }
        public decimal? ProductVolume { get; set; }
        public string MeasurmentUnit { get; set; }
        public string SystemName { get; set; }
        public decimal? AttributeValue { get; set; }
        public string ValueAttributeUnit { get; set; }
        public string AttributeDescription { get; set; }
        public int? TicketId { get; set; }
        public long? Rno { get; set; }
    }
}
