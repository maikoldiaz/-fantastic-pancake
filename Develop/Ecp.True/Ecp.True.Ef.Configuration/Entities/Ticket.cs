using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Ticket
    {
        public Ticket()
        {
            Inventory = new HashSet<Inventory>();
            InventoryProductOwnershipTicket = new HashSet<InventoryProduct>();
            InventoryProductTicket = new HashSet<InventoryProduct>();
            MovementOwnershipTicket = new HashSet<Movement>();
            MovementTicket = new HashSet<Movement>();
            Ownership = new HashSet<Ownership>();
            OwnershipAnalytics = new HashSet<OwnershipAnalytics>();
            OwnershipCalculation = new HashSet<OwnershipCalculation>();
            OwnershipNode = new HashSet<OwnershipNode>();
            PendingTransaction = new HashSet<PendingTransaction>();
            RegistrationProgress = new HashSet<RegistrationProgress>();
            SegmentOwnershipCalculation = new HashSet<SegmentOwnershipCalculation>();
            SegmentUnbalance = new HashSet<SegmentUnbalance>();
            SystemOwnershipCalculation = new HashSet<SystemOwnershipCalculation>();
            SystemUnbalance = new HashSet<SystemUnbalance>();
            Unbalance = new HashSet<Unbalance>();
            UnbalanceComment = new HashSet<UnbalanceComment>();
        }

        public int TicketId { get; set; }
        public int CategoryElementId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public int? TicketTypeId { get; set; }
        public string TicketGroupId { get; set; }
        public string ErrorMessage { get; set; }
        public int? OwnerId { get; set; }
        public int? NodeId { get; set; }
        public string BlobPath { get; set; }
        public int? AnalyticsStatus { get; set; }
        public string AnalyticsErrorMessage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual CategoryElement CategoryElement { get; set; }
        public virtual Node Node { get; set; }
        public virtual CategoryElement Owner { get; set; }
        public virtual StatusType StatusNavigation { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<InventoryProduct> InventoryProductOwnershipTicket { get; set; }
        public virtual ICollection<InventoryProduct> InventoryProductTicket { get; set; }
        public virtual ICollection<Movement> MovementOwnershipTicket { get; set; }
        public virtual ICollection<Movement> MovementTicket { get; set; }
        public virtual ICollection<Ownership> Ownership { get; set; }
        public virtual ICollection<OwnershipAnalytics> OwnershipAnalytics { get; set; }
        public virtual ICollection<OwnershipCalculation> OwnershipCalculation { get; set; }
        public virtual ICollection<OwnershipNode> OwnershipNode { get; set; }
        public virtual ICollection<PendingTransaction> PendingTransaction { get; set; }
        public virtual ICollection<RegistrationProgress> RegistrationProgress { get; set; }
        public virtual ICollection<SegmentOwnershipCalculation> SegmentOwnershipCalculation { get; set; }
        public virtual ICollection<SegmentUnbalance> SegmentUnbalance { get; set; }
        public virtual ICollection<SystemOwnershipCalculation> SystemOwnershipCalculation { get; set; }
        public virtual ICollection<SystemUnbalance> SystemUnbalance { get; set; }
        public virtual ICollection<Unbalance> Unbalance { get; set; }
        public virtual ICollection<UnbalanceComment> UnbalanceComment { get; set; }
    }
}
