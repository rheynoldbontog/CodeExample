using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class rfqstatu
    {
        public rfqstatu()
        {
            this.rfqs = new List<rfq>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public System.DateTime DateCreatedOnUtc { get; set; }
        public System.DateTime DateUpdatedOnUtc { get; set; }
        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public virtual ICollection<rfq> rfqs { get; set; }
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
    }
}
