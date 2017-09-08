using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSG.Core.Domain.RFQ
{
    public partial class EquipmentCalibrationType : BaseAuditEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        //Relationships
        #region RFQLine

        public virtual ICollection<RFQLine> RFQLines { get; set; }

        #endregion
    }
}
