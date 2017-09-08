using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSG.Core.Domain.RFQ
{
    public class RFQLineStatusHistory : BaseAuditEntity
    {
        public virtual bool Active  { get; set; }

        #region RFQLine
        
        public virtual int RFQLineId { get; set; }
        
        public virtual RFQLine RFQLine { get; set; }
        
        #endregion

        #region "Old Status"

        public int OldStatusId { get; set; }

        public virtual RFQLineStatus OldStatus { get; set; }

        #endregion

        #region "New Status"

        public int NewStatusId { get; set; }

        public virtual RFQLineStatus NewStatus { get; set; }

        #endregion
    }
}
