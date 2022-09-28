using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ContractInformation
    {
        public int DocumentNumber { get; set; }
        public int Position { get; set; }
        public string TypeOfMovement { get; set; }
        public string SourceNode { get; set; }
        public string DestinationNode { get; set; }
        public string Product { get; set; }
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
