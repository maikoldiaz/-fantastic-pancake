using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class PendingTransaction
    {
        public PendingTransaction()
        {
            PendingTransactionError = new HashSet<PendingTransactionError>();
        }

        public int TransactionId { get; set; }
        public int? MessageTypeId { get; set; }
        public string BlobName { get; set; }
        public string MessageId { get; set; }
        public string ErrorJson { get; set; }
        public string SourceNodeId { get; set; }
        public string DestinationNodeId { get; set; }
        public string SourceProductId { get; set; }
        public string DestinationProductId { get; set; }
        public int? ActionTypeId { get; set; }
        public string Volume { get; set; }
        public string Units { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TicketId { get; set; }
        public int? SystemTypeId { get; set; }
        public string SystemName { get; set; }
        public int? OwnerId { get; set; }
        public int? SegmentId { get; set; }
        public int? TypeId { get; set; }
        public string Identifier { get; set; }
        public string Type { get; set; }
        public string Messagetype { get; set; }
        public string ActionType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual RegisterFileActionType ActionTypeNavigation { get; set; }
        public virtual MessageType MessageType { get; set; }
        public virtual CategoryElement Owner { get; set; }
        public virtual CategoryElement Segment { get; set; }
        public virtual SystemType SystemType { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual CategoryElement TypeNavigation { get; set; }
        public virtual ICollection<PendingTransactionError> PendingTransactionError { get; set; }
    }
}
