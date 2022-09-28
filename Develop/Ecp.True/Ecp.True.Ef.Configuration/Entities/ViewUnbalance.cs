using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewUnbalance
    {
        public int UnbalanceId { get; set; }
        public int NodeId { get; set; }
        public string ProductId { get; set; }
        public string Product { get; set; }
        public decimal InitialInventory { get; set; }
        public decimal Inputs { get; set; }
        public decimal Outputs { get; set; }
        public decimal IdentifiedLosses { get; set; }
        public decimal FinalInvnetory { get; set; }
        public decimal? Unbalance { get; set; }
        public decimal Interface { get; set; }
        public decimal? Tolerance { get; set; }
        public decimal? UnidentifiedLosses { get; set; }
        public string MeasurementUnit { get; set; }
        public int TicketId { get; set; }
        public string NodeName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
