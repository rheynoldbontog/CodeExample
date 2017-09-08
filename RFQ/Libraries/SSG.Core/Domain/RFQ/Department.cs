using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.Users;

namespace SSG.Core.Domain.RFQ
{
    public partial class Department : BaseAuditEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        //Relationships
        public virtual ICollection<User> Users {get; set;}

        public virtual ICollection<User> Buyers { get; set; }
    }
}
