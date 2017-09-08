using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class uom
    {
        public uom()
        {
            this.rfqlines = new List<rfqline>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public System.DateTime DateCreatedOnUtc { get; set; }
        public System.DateTime DateUpdatedOnUtc { get; set; }
        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public virtual ICollection<rfqline> rfqlines { get; set; }
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
    }
}
