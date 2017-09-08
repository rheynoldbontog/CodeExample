using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class department
    {
        public department()
        {
            this.users = new List<user>();
            this.users1 = new List<user>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public System.DateTime DateCreatedOnUtc { get; set; }
        public System.DateTime DateUpdatedOnUtc { get; set; }
        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
        public virtual ICollection<user> users { get; set; }
        public virtual ICollection<user> users1 { get; set; }
    }
}
