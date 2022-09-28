using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class SystemUnbalance
    {
        public int SystemUnbalanceId { get; set; }
        public int SystemId { get; set; }
        public int SegmentId { get; set; }
        public string ProductId { get; set; }
        public int? TicketId { get; set; }
        public decimal? InitialInventoryVolume { get; set; }
        public decimal? FinalInventoryVolume { get; set; }
        public decimal? InputVolume { get; set; }
        public decimal? OutputVolume { get; set; }
        public decimal? IdentifiedLossesVolume { get; set; }
        public decimal? UnbalanceVolume { get; set; }
        public decimal? InterfaceVolume { get; set; }
        public decimal? ToleranceVolume { get; set; }
        public decimal? UnidentifiedLossesVolume { get; set; }
        public DateTime Date { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual Product Product { get; set; }
        public virtual CategoryElement Segment { get; set; }
        public virtual CategoryElement System { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
