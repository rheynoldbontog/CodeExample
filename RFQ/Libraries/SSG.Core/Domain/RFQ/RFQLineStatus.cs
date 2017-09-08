using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSG.Core.Domain.RFQ
{
    public enum RFQLineStates
    {
        Open = 1,
        Closed = 2,
        Deleted = 3
    }

    public partial class RFQLineStatus : BaseAuditEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        //Relationships
        public virtual ICollection<RFQLine> Lines { get; set; }
    }
}
