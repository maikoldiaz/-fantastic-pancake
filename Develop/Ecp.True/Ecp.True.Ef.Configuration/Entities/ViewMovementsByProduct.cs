using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewMovementsByProduct
    {
        public string FilterType { get; set; }
        public decimal? InitialInventory { get; set; }
        public decimal? Input { get; set; }
        public decimal? Output { get; set; }
        public decimal? IdentifiedLosses { get; set; }
        public decimal? Interface { get; set; }
        public decimal? Tolerance { get; set; }
        public decimal? UnidentifiedLosses { get; set; }
        public decimal? FinalInventory { get; set; }
        public decimal? Control { get; set; }
        public string Category { get; set; }
        public string Element { get; set; }
        public string NodeName { get; set; }
        public DateTime? CalculationDate { get; set; }
        public string Product { get; set; }
        public string ProductId { get; set; }
        public int? TicketId { get; set; }
        public int? NodeId { get; set; }
        public int? SegmentId { get; set; }
        public int? SystemId { get; set; }
        public int PercentageValue { get; set; }
    }
}
