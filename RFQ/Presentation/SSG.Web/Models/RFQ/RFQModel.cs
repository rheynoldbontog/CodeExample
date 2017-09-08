using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;
using SSG.Web.Validators.User;
using System;
using SSG.Web.Validators.RFQ;

namespace SSG.Web.Models.RFQ
{
    [Validator(typeof(RFQValidator))]
    public partial class RFQModel
    {
        public RFQModel()
        {
            this.BuyersList = new List<SelectListItem>();
            this.Lines = new List<RFQLineModel>();
        }

        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [SSGResourceDisplayName("RFQ No.")]
        public string RFQNo { get; set; }

        [SSGResourceDisplayName("Department")]
        [AllowHtml]
        public string Department { get; set; }

        [SSGResourceDisplayName("Requester")]
        [AllowHtml]
        public int RequesterId { get; set; }
        public string Requester { get; set; }
        //public IList<SelectListItem> RequestersList { get; set; }
                
        [SSGResourceDisplayName("Subject")]
        public string Subject { get; set; }

        [SSGResourceDisplayName("Buyer")]
        [AllowHtml]
        public int BuyerId { get; set; }
        public string Buyer { get; set; }
        public IList<SelectListItem> BuyersList { get; set; }
        
        [SSGResourceDisplayName("Active")]
        public bool Active { get; set; }

        // REFERENCES
        [SSGResourceDisplayName("RFQ Status")]
        public int StatusId { get; set; }
        public string Status { get; set; }

        public int LastLineNo { get; set; }
        public List<RFQLineModel> Lines { get; set; }

        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }

        [SSGResourceDisplayName("RFQ Creation Date")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime DateCreatedOnUtc { get; set; }
        public DateTime DateUpdatedOnUtc { get; set; }

        public DateTime? SubmittedDate { get; set; }

        #region "Business Logic Properties"

        public bool UserCanSubmit { get; set; }

        public bool UserCanAddLine { get; set; }

        public bool UserCanEdit { get; set; }

        public bool UserIsRequestor { get; set; }

        public bool UserIsBuyer { get; set; }

        public bool IsCopy { get; set; }

        public bool IsSubmitted { get; set; }

        #endregion
    }
}