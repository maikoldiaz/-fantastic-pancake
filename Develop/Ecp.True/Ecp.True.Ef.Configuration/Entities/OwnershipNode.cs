using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OwnershipNode
    {
        public OwnershipNode()
        {
            OwnershipNodeError = new HashSet<OwnershipNodeError>();
        }

        public int OwnershipNodeId { get; set; }
        public int TicketId { get; set; }
        public int NodeId { get; set; }
        public int Status { get; set; }
        public int? OwnershipStatusId { get; set; }
        public string Editor { get; set; }
        public int? ReasonId { get; set; }
        public string Comment { get; set; }
        public string EditorConnectionId { get; set; }
        public string ApproverAlias { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual Node Node { get; set; }
        public virtual OwnershipNodeStatusType OwnershipStatus { get; set; }
        public virtual CategoryElement Reason { get; set; }
        public virtual StatusType StatusNavigation { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual ICollection<OwnershipNodeError> OwnershipNodeError { get; set; }
    }
}
