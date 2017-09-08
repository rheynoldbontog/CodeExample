using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;
using SSG.Web.Validators.User;

namespace SSG.Web.Models.User
{
    [Validator(typeof(ChangePasswordValidator))]
    public partial class ChangePasswordModel : BaseSSGModel
    {
        [AllowHtml]
        [DataType(DataType.Password)]
        [SSGResourceDisplayName("Account.ChangePassword.Fields.OldPassword")]
        public string OldPassword { get; set; }

        [AllowHtml]
        [DataType(DataType.Password)]
        [SSGResourceDisplayName("Account.ChangePassword.Fields.NewPassword")]
        public string NewPassword { get; set; }

        [AllowHtml]
        [DataType(DataType.Password)]
        [SSGResourceDisplayName("Account.ChangePassword.Fields.ConfirmNewPassword")]
        public string ConfirmNewPassword { get; set; }

        public string Result { get; set; }

        public UserNavigationModel NavigationModel { get; set; }

    }
}