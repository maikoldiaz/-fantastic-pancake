using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class EventInformation
    {
        public string PropertyEvent { get; set; }
        public string SourceNode { get; set; }
        public string DestinationNode { get; set; }
        public string SourceProduct { get; set; }
        public string DestinationProduct { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Owner1Name { get; set; }
        public string Owner2Name { get; set; }
        public decimal Volume { get; set; }
        public string MeasurementUnit { get; set; }
        public string Element { get; set; }
        public string NodeName { get; set; }
        public long? Rno { get; set; }
    }
}
