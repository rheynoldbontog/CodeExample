using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSG.Core.Domain.RFQ
{
    public partial class EquipmentPurchaseType : BaseAuditEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        //REFERENCES
        #region RFQLine

        public virtual ICollection<RFQLine> RFQLines { get; set; }

        #endregion
    }
}
