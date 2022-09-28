using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class NodeTag
    {
        public int NodeTagId { get; set; }
        public int NodeId { get; set; }
        public int ElementId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual CategoryElement Element { get; set; }
        public virtual Node Node { get; set; }
    }
}
