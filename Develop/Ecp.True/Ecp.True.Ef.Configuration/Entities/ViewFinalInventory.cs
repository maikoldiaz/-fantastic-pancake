using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewFinalInventory
    {
        public string Category { get; set; }
        public string Element { get; set; }
        public string NodeName { get; set; }
        public DateTime? CalculationDate { get; set; }
        public string ProductId { get; set; }
        public string Product { get; set; }
        public int PercentageValue { get; set; }
        public int TicketId { get; set; }
        public int NodeId { get; set; }
        public long? Rnum { get; set; }
    }
}
