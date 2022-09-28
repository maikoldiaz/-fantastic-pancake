using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewUnBalanceTicket
    {
        public int TicketId { get; set; }
        public int CategoryElementId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
