using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSG.Core.Domain.PSP
{
    public partial class SupportCode : BaseAuditEntity
    {
        public string SupportCodes { get; set; }
        public string SupportCodeDescription { get; set; }
        public bool Active { get; set; }
    }
}
