using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class Version
    {
        public int VersionId { get; set; }
        public int Number { get; set; }
        public string Type { get; set; }
    }
}
