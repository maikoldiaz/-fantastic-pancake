using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewNodeConnectionProductRule
    {
        public int NodeConnectionProductid { get; set; }
        public string SourceOperator { get; set; }
        public string DestinationOperator { get; set; }
        public string SourceNode { get; set; }
        public string DestinationNode { get; set; }
        public string Product { get; set; }
        public int? RuleId { get; set; }
        public string RuleName { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
