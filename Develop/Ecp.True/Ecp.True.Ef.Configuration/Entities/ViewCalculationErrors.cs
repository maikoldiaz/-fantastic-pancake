using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewCalculationErrors
    {
        public int OperationId { get; set; }
        public string Type { get; set; }
        public int OwnershipNodeId { get; set; }
        public string Operation { get; set; }
        public DateTime OperationDate { get; set; }
        public DateTime? ExecutionDate { get; set; }
        public string Segment { get; set; }
        public decimal? NetVolume { get; set; }
        public string ProductOrigin { get; set; }
        public string ProductDestination { get; set; }
        public string ErrorMessage { get; set; }
    }
}
