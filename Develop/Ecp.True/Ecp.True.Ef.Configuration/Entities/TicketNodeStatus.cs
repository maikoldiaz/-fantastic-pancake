using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class TicketNodeStatus
    {
        public int? OwnershipNodeId { get; set; }
        public int? TicketId { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }
        public int? NodeId { get; set; }
        public int? SegmentId { get; set; }
        public string SegmentName { get; set; }
        public int? SystemId { get; set; }
        public string SystemName { get; set; }
        public string NodeName { get; set; }
        public int OwnershipNodeStatusId { get; set; }
        public string StatusNode { get; set; }
        public DateTime? StatusDateChange { get; set; }
        public string Approver { get; set; }
        public string Comment { get; set; }
        public string ExecutionId { get; set; }
        public int ReportConfiguartionValue { get; set; }
        public string CalculatedDays { get; set; }
        public int NotInApprovedState { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
