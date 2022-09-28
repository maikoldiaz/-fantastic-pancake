using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Operational
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SegmentId { get; set; }
        public string SegmentName { get; set; }
        public int? NodeId { get; set; }
        public DateTime CalculationDate { get; set; }
        public decimal? Inputs { get; set; }
        public decimal? Outputs { get; set; }
        public decimal? IdentifiedLosses { get; set; }
        public decimal? IntialInventory { get; set; }
        public decimal? FinalInventory { get; set; }
        public decimal? UnBalance { get; set; }
        public string InputCategory { get; set; }
        public string InputElementName { get; set; }
        public string InputNodeName { get; set; }
        public DateTime InputStartDate { get; set; }
        public DateTime InputEndDate { get; set; }
        public string ExecutionId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
