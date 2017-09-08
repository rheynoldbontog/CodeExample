using System.Collections.Generic;
using System.Web.Mvc;
using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;
using Telerik.Web.Mvc;

namespace SSG.Admin.Models.Users
{
    public partial class UserListModel : BaseSSGModel
    {
        public GridModel<UserModel> Users { get; set; }

        [SSGResourceDisplayName("Admin.Users.Users.List.UserRoles")]
        [AllowHtml]
        public List<UserRoleModel> AvailableUserRoles { get; set; }

        [SSGResourceDisplayName("Admin.Users.Users.List.UserRoles")]
        public int[] SearchUserRoleIds { get; set; }

        [SSGResourceDisplayName("Admin.Users.Users.List.SearchEmail")]
        [AllowHtml]
        public string SearchEmail { get; set; }

        [SSGResourceDisplayName("Admin.Users.Users.List.SearchUsername")]
        [AllowHtml]
        public string SearchUsername { get; set; }
        public bool UsernamesEnabled { get; set; }

        [SSGResourceDisplayName("Admin.Users.Users.List.SearchFirstName")]
        [AllowHtml]
        public string SearchFirstName { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.List.SearchLastName")]
        [AllowHtml]
        public string SearchLastName { get; set; }


        [SSGResourceDisplayName("Admin.Users.Users.List.SearchDateOfBirth")]
        [AllowHtml]
        public string SearchDayOfBirth { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.List.SearchDateOfBirth")]
        [AllowHtml]
        public string SearchMonthOfBirth { get; set; }
        public bool DateOfBirthEnabled { get; set; }



        [SSGResourceDisplayName("Admin.Users.Users.List.SearchCompany")]
        [AllowHtml]
        public string SearchCompany { get; set; }
        public bool CompanyEnabled { get; set; }

        [SSGResourceDisplayName("Admin.Users.Users.List.SearchPhone")]
        [AllowHtml]
        public string SearchPhone { get; set; }
        public bool PhoneEnabled { get; set; }

        [SSGResourceDisplayName("Admin.Users.Users.List.SearchZipCode")]
        [AllowHtml]
        public string SearchZipPostalCode { get; set; }
        public bool ZipPostalCodeEnabled { get; set; }
    }
}