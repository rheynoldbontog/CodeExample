using System;
using System.Web.Mvc;
using FluentValidation.Attributes;
using SSG.Admin.Validators.Messages;
using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;

namespace SSG.Admin.Models.Messages
{
    [Validator(typeof(NewsLetterSubscriptionValidator))]
    public partial class NewsLetterSubscriptionModel : BaseSSGEntityModel
    {
        [SSGResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.Email")]
        [AllowHtml]
        public string Email { get; set; }

        [SSGResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.Active")]
        public bool Active { get; set; }

        [SSGResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }
}