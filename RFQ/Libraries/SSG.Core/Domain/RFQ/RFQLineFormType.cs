using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSG.Core.Domain.RFQ
{
    public enum RFQLineFormTypes
    {
       GeneralForm = 1,
       TestEquipmentRentalAndCalibrationForm = 2
    }

    public partial class RFQLineFormType : BaseAuditEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        //Relationships
        public virtual ICollection<RFQLine> Lines { get; set; }
    }
}
