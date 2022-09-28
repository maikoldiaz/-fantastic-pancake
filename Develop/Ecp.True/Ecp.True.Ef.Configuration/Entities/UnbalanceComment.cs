using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class UnbalanceComment
    {
        public int UnbalanceId { get; set; }
        public int TicketId { get; set; }
        public int NodeId { get; set; }
        public string ProductId { get; set; }
        public decimal Unbalance { get; set; }
        public string Units { get; set; }
        public decimal UnbalancePercentage { get; set; }
        public decimal ControlLimit { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }
        public DateTime CalculationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Node Node { get; set; }
        public virtual Product Product { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
