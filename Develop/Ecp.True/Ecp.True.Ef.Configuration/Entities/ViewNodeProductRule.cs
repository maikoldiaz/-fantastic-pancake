using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class ViewNodeProductRule
    {
        public string Segment { get; set; }
        public string Operator { get; set; }
        public string NodeType { get; set; }
        public string NodeName { get; set; }
        public string StorageLocation { get; set; }
        public string Product { get; set; }
        public int? RuleId { get; set; }
        public string RuleName { get; set; }
        public int StorageLocationProductId { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
