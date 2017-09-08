using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSG.Core.Domain.RFQ
{
    public enum RFQStates
    {
        Draft = 1,
        Submitted = 2
    }

    public partial class RFQStatus : BaseAuditEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        //REFERENCES
        #region RFQ

        public virtual ICollection<RFQ> QuoteRequests { get; set; }
        
        #endregion


    }
}
