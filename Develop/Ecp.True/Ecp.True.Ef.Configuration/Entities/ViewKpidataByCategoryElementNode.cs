using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewKpidataByCategoryElementNode
    {
        public string FilterType { get; set; }
        public int OrderToDisplay { get; set; }
        public string Category { get; set; }
        public string Element { get; set; }
        public string NodeName { get; set; }
        public int? NodeId { get; set; }
        public DateTime? CalculationDate { get; set; }
        public string Product { get; set; }
        public string Indicator { get; set; }
        public decimal? CurrentValue { get; set; }
        public int? TicketId { get; set; }
    }
}
