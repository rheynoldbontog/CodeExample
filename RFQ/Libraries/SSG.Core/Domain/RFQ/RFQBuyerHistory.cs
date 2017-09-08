using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.Users;

namespace SSG.Core.Domain.RFQ
{
    public class RFQBuyerHistory : BaseAuditEntity
    {
        public virtual int RFQId { get; set; }

        public virtual RFQ RFQ { get; set; }

        public virtual int OldBuyerId { get; set; }

        public virtual User OldBuyer { get; set; }

        public virtual int NewBuyerId { get; set; }

        public virtual User NewBuyer { get; set; }

        public virtual string ChangeReason { get; set; }

        public virtual bool Active { get; set; }
    }
}
