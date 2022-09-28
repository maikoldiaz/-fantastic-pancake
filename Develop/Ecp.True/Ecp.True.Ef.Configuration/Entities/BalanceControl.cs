using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class BalanceControl
    {
        public string Category { get; set; }
        public string Element { get; set; }
        public string NodeName { get; set; }
        public DateTime? CalculationDate { get; set; }
        public string Product { get; set; }
        public string ProductId { get; set; }
        public int TicketId { get; set; }
        public int NodeId { get; set; }
        public decimal? Input { get; set; }
        public decimal? Unbalance { get; set; }
        public decimal? StandardUncertainty { get; set; }
        public decimal? AverageUncertainty { get; set; }
        public decimal? AverageUncertaintyUnbalance { get; set; }
        public decimal? Warning { get; set; }
        public decimal? Action { get; set; }
        public decimal? ControlTolerance { get; set; }
        public decimal? Warning1 { get; set; }
        public decimal? Action1 { get; set; }
        public decimal? ControlTolerance1 { get; set; }
    }
}
