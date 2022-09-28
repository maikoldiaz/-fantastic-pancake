using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class RegistrationProgress
    {
        public int RegistrationProgressId { get; set; }
        public int TicketId { get; set; }
        public string MovementTransactionId { get; set; }
        public int? InventoryProductId { get; set; }
        public bool Status { get; set; }
        public int MessageTypeId { get; set; }
        public string Step { get; set; }
        public string Action { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual MessageType MessageType { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
