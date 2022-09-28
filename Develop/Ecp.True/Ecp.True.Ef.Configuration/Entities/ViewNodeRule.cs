using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewNodeRule
    {
        public int NodeId { get; set; }
        public string Name { get; set; }
        public string Segment { get; set; }
        public string Operator { get; set; }
        public string NodeType { get; set; }
        public int? RuleId { get; set; }
        public string RuleName { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
