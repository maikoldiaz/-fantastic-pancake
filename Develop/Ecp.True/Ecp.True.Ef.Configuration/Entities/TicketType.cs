using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class TicketType
    {
        public TicketType()
        {
            Ticket = new HashSet<Ticket>();
        }

        public int TicketTypeId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}
