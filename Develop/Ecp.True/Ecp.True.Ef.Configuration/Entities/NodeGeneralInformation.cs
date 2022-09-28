using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class NodeGeneralInformation
    {
        public string NodeName { get; set; }
        public string NodeOrder { get; set; }
        public int NodeId { get; set; }
        public string NodeOwnershipStrategy { get; set; }
        public string NodeControlLimit { get; set; }
        public string NodeAcceptableBalancePercentage { get; set; }
        public string Element { get; set; }
        public string Category { get; set; }
    }
}
