using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewOperativeMovementsPeriodic
    {
        public DateTime OperationalDate { get; set; }
        public string DestinationNode { get; set; }
        public string DestinationNodeType { get; set; }
        public string MovementType { get; set; }
        public string SourceNode { get; set; }
        public string SourceNodeType { get; set; }
        public string SourceProduct { get; set; }
        public string SourceProductType { get; set; }
        public decimal NetStandardVolume { get; set; }
    }
}
