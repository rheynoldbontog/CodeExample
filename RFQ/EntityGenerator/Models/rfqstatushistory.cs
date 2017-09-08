using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class rfqstatushistory
    {
        public int Id { get; set; }
        public int RfqLineId { get; set; }
        public int RfqStatusId { get; set; }
        public Nullable<System.DateTime> DateCreatedOnUtc { get; set; }
        public Nullable<System.DateTime> DateUpdatedOnUtc { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public Nullable<int> UpdatedByUserId { get; set; }
    }
}
