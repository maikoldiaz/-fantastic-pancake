using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class FileRegistrationError
    {
        public int FileRegistrationErrorId { get; set; }
        public int FileRegistrationId { get; set; }
        public string ErrorMessage { get; set; }
        public string MessageId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual FileRegistration FileRegistration { get; set; }
    }
}
