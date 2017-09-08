using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class rfq
    {
        public rfq()
        {
            this.rfqlines = new List<rfqline>();
        }

        public int Id { get; set; }
        public string RFQNo { get; set; }
        public string Department { get; set; }
        public System.DateTime NeedByDate { get; set; }
        public string Subject { get; set; }
        public string NoteToBuyer { get; set; }
        public bool Active { get; set; }
        public int RFQStatusId { get; set; }
        public int BuyerId { get; set; }
        public int RequestorId { get; set; }
        public System.DateTime DateCreatedOnUtc { get; set; }
        public System.DateTime DateUpdatedOnUtc { get; set; }
        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
        public virtual user user2 { get; set; }
        public virtual user user3 { get; set; }
        public virtual ICollection<rfqline> rfqlines { get; set; }
        public virtual rfqstatu rfqstatu { get; set; }
    }
}
