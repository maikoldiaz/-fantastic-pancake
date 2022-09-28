using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewTicket
    {
        public int TicketId { get; set; }
        public int? TicketTypeId { get; set; }
        public string Segment { get; set; }
        public string CategoryName { get; set; }
        public DateTime TicketStartDate { get; set; }
        public DateTime TicketFinalDate { get; set; }
        public DateTime CutoffExecutionDate { get; set; }
        public string CreatedBy { get; set; }
        public string OwnerName { get; set; }
        public string ErrorMessage { get; set; }
        public string BlobPath { get; set; }
        public string State { get; set; }
        public string NodeName { get; set; }
    }
}
