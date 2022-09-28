using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class OwnerColor
    {
        public int OwnerColorId { get; set; }
        public int OwnerId { get; set; }
        public int ColorId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
