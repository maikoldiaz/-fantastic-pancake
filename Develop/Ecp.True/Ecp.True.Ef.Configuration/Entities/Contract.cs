using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Contract
    {
        public int ContractId { get; set; }
        public int DocumentNumber { get; set; }
        public int Position { get; set; }
        public int MovementTypeId { get; set; }
        public int SourceNodeId { get; set; }
        public int DestinationNodeId { get; set; }
        public string ProductId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Owner1Id { get; set; }
        public int Owner2Id { get; set; }
        public decimal Volume { get; set; }
        public int MeasurementUnit { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual Node DestinationNode { get; set; }
        public virtual CategoryElement MeasurementUnitNavigation { get; set; }
        public virtual CategoryElement MovementType { get; set; }
        public virtual CategoryElement Owner1 { get; set; }
        public virtual CategoryElement Owner2 { get; set; }
        public virtual Product Product { get; set; }
        public virtual Node SourceNode { get; set; }
    }
}
