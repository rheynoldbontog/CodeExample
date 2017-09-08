using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.Users;

namespace SSG.Core.Domain.RFQ
{
    public partial class RFQ : BaseAuditEntity
    {
        public string RFQNo { get; set; }

        public string Department { get; set; }

        public DateTime NeedByDate { get; set; }

        public string Subject { get; set; }

        public string NoteToBuyer { get; set; }

        public bool Active { get; set; }

        public DateTime? SubmittedDate { get; set; }

        //REFERENCES

        #region Status Columns
        
        public virtual int RFQStatusId { get; set; }
        public virtual RFQStatus RFQStatus { get; set; }

        #endregion

        #region Buyer Columns

        public virtual int BuyerId { get; set; }
        public virtual User Buyer { get; set; }

        #endregion

        #region Requestor Columns

        public virtual int RequestorId { get; set; }
        public virtual User Requestor { get; set; }

        #endregion

        #region RFQLines

        public virtual ICollection<RFQLine> Lines { get; set; }

        #endregion

        public virtual ICollection<RFQBuyerHistory> BuyerChanges { get; set; }

        public virtual string EmailAttachmentFilename { get; set; }

        public virtual string EditUrl { get; set; }

        #region Helper Properties

        public virtual string BuyerChangeReason { get; set; }

        #endregion
    }
}
