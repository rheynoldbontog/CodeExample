using System.Web.Mvc;
using FluentValidation.Attributes;
using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;
using SSG.Web.Validators.User;

namespace SSG.Web.Models.User
{
    [Validator(typeof(PasswordRecoveryValidator))]
    public partial class PasswordRecoveryModel : BaseSSGModel
    {
        [AllowHtml]
        [SSGResourceDisplayName("Account.PasswordRecovery.Email")]
        public string Email { get; set; }

        [SSGResourceDisplayName("Account.PasswordRecovery.EmployeeID")]
        [AllowHtml]
        public string EmployeeId { get; set; }

        public string Result { get; set; }
    }
}