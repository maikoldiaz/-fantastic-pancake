using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Inventory
    {
        public int InventoryTransactionId { get; set; }
        public int SystemTypeId { get; set; }
        public string SystemName { get; set; }
        public string SourceSystem { get; set; }
        public string DestinationSystem { get; set; }
        public string EventType { get; set; }
        public string TankName { get; set; }
        public string InventoryId { get; set; }
        public int? TicketId { get; set; }
        public DateTime InventoryDate { get; set; }
        public int NodeId { get; set; }
        public int? SegmentId { get; set; }
        public string Observations { get; set; }
        public string Scenario { get; set; }
        public bool IsDeleted { get; set; }
        public int? FileRegistrationTransactionId { get; set; }
        public decimal? UncertaintyPercentage { get; set; }
        public string Operator { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual FileRegistrationTransaction FileRegistrationTransaction { get; set; }
        public virtual Node Node { get; set; }
        public virtual CategoryElement Segment { get; set; }
        public virtual SystemType SystemType { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
