using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class permissionrecord
    {
        public permissionrecord()
        {
            this.userroles = new List<userrole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string SystemName { get; set; }
        public string Category { get; set; }
        public virtual ICollection<userrole> userroles { get; set; }
    }
}
