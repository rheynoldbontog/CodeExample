using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSG.Core.Domain.RFQ
{
    public enum RFQLineTypes
    {
        Goods = 1,
        AMT2 = 2
    };

    public partial class RFQLineType : BaseAuditEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        //Relationships
        public virtual ICollection<RFQLine> Lines { get; set; }
    }
}
