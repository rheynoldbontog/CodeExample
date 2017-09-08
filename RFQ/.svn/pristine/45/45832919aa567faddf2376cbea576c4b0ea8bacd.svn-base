using System.Web.Mvc;
using FluentValidation.Attributes;
using SSG.Admin.Validators.Users;
using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;

namespace SSG.Admin.Models.Users
{
    [Validator(typeof(UserRoleValidator))]
    public partial class UserRoleModel : BaseSSGEntityModel
    {
        [SSGResourceDisplayName("Admin.Users.UserRoles.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [SSGResourceDisplayName("Admin.Users.UserRoles.Fields.Active")]
        public bool Active { get; set; }

        [SSGResourceDisplayName("Admin.Users.UserRoles.Fields.IsSystemRole")]
        public bool IsSystemRole { get; set; }

        [SSGResourceDisplayName("Admin.Users.UserRoles.Fields.SystemName")]
        public string SystemName { get; set; }
    }
}