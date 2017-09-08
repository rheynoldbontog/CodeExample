using System.ComponentModel.DataAnnotations;
using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;

namespace SSG.Web.Models.User
{
    public partial class LoginModel : BaseSSGModel
    {
        public bool CheckoutAsGuest { get; set; }

        [SSGResourceDisplayName("Account.Login.Fields.Email")]
        public string Email { get; set; }

        public bool UsernamesEnabled { get; set; }
        [SSGResourceDisplayName("Account.Login.Fields.UserName")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [SSGResourceDisplayName("Account.Login.Fields.Password")]
        public string Password { get; set; }

        [SSGResourceDisplayName("Account.Login.Fields.RememberMe")]
        public bool RememberMe { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}