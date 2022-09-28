using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OperationalInventory
    {
        public int Rno { get; set; }
        public string InventoryId { get; set; }
        public DateTime CalculationDate { get; set; }
        public string NodeName { get; set; }
        public string TankName { get; set; }
        public string BatchId { get; set; }
        public string Product { get; set; }
        public string ProductId { get; set; }
        public decimal? NetStandardVolume { get; set; }
        public string MeasurementUnit { get; set; }
        public string SystemName { get; set; }
        public decimal? PercentStandardUnCertainty { get; set; }
        public string InputCategory { get; set; }
        public string InputElementName { get; set; }
        public string InputNodeName { get; set; }
        public DateTime InputStartDate { get; set; }
        public DateTime InputEndDate { get; set; }
        public string ExecutionId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
