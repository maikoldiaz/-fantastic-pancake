using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class MovementEvent
    {
        public MovementEvent()
        {
            Movement = new HashSet<Movement>();
        }

        public int MovementEventId { get; set; }
        public int EventTypeId { get; set; }
        public int SourceNodeId { get; set; }
        public int DestinationNodeId { get; set; }
        public string SourceProductId { get; set; }
        public string DestinationProductId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Owner1Id { get; set; }
        public int Owner2Id { get; set; }
        public decimal Volume { get; set; }
        public string MeasurementUnit { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual Node DestinationNode { get; set; }
        public virtual Product DestinationProduct { get; set; }
        public virtual CategoryElement EventType { get; set; }
        public virtual CategoryElement Owner1 { get; set; }
        public virtual CategoryElement Owner2 { get; set; }
        public virtual Node SourceNode { get; set; }
        public virtual Product SourceProduct { get; set; }
        public virtual ICollection<Movement> Movement { get; set; }
    }
}
