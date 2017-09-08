using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class userrole
    {
        public userrole()
        {
            this.permissionrecords = new List<permissionrecord>();
            this.users = new List<user>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool FreeShipping { get; set; }
        public bool TaxExempt { get; set; }
        public bool Active { get; set; }
        public bool IsSystemRole { get; set; }
        public string SystemName { get; set; }
        public virtual ICollection<permissionrecord> permissionrecords { get; set; }
        public virtual ICollection<user> users { get; set; }
    }
}
