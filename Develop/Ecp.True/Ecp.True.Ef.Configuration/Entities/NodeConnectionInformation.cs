using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class NodeConnectionInformation
    {
        public string ConnectionType { get; set; }
        public string NodeName { get; set; }
        public string NodeConnectionName { get; set; }
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public int? Priority { get; set; }
        public string TransferPoint { get; set; }
        public string AlgorithmName { get; set; }
        public string OwnershipStrategy { get; set; }
        public decimal? UncertaintyConnectionProduct { get; set; }
        public decimal? OwnershipPercentage { get; set; }
        public string OwnerName { get; set; }
        public string Category { get; set; }
        public string Element { get; set; }
        public int NodeId { get; set; }
        public long? Rno { get; set; }
    }
}
