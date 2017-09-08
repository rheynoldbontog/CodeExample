using System.Web.Mvc;
using FluentValidation.Attributes;
using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;
using SSG.Web.Validators.Common;

namespace SSG.Web.Models.Common
{
    [Validator(typeof(ContactUsValidator))]
    public partial class ContactUsModel : BaseSSGModel
    {
        [AllowHtml]
        [SSGResourceDisplayName("ContactUs.Email")]
        public string Email { get; set; }

        [AllowHtml]
        [SSGResourceDisplayName("ContactUs.Enquiry")]
        public string Enquiry { get; set; }

        [AllowHtml]
        [SSGResourceDisplayName("ContactUs.FullName")]
        public string FullName { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}