using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using SSG.Admin.Validators.Users;
using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;
using SSG.Core.Domain.RFQ;

namespace SSG.Admin.Models.Users
{
    [Validator(typeof(UserValidator))]
    public partial class UserModel : BaseSSGEntityModel
    {
        public UserModel()
        {
            AvailableTimeZones = new List<SelectListItem>();
            SendEmail = new SendEmailModel();
            SendPm = new SendPmModel();
            AvailableUserRoles = new List<UserRoleModel>();
            AssociatedExternalAuthRecords = new List<AssociatedExternalAuthModel>();
            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
            AvailableDepartments = new List<SelectListItem>();
            
        }

        public bool AllowUsersToChangeUsernames { get; set; }
        public bool UsernamesEnabled { get; set; }

        [SSGResourceDisplayName("Admin.Users.Users.Fields.Username")]
        [AllowHtml]
        public string Username { get; set; }

        [SSGResourceDisplayName("Admin.Users.Users.Fields.Email")]
        [AllowHtml]
        public string Email { get; set; }

        [SSGResourceDisplayName("Admin.Users.Users.Fields.Password")]
        [AllowHtml]
        public string Password { get; set; }

        //form fields & properties
        public bool GenderEnabled { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.Fields.Gender")]
        public string Gender { get; set; }

        [SSGResourceDisplayName("Admin.Users.Users.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.Fields.FullName")]
        public string FullName { get; set; }
        
        public bool DateOfBirthEnabled { get; set; }
        [UIHint("DateNullable")]
        [SSGResourceDisplayName("Admin.Users.Users.Fields.DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        public bool CompanyEnabled { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.Fields.Company")]
        [AllowHtml]
        public string Company { get; set; }

        public bool StreetAddressEnabled { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.Fields.StreetAddress")]
        [AllowHtml]
        public string StreetAddress { get; set; }

        public bool StreetAddress2Enabled { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.Fields.StreetAddress2")]
        [AllowHtml]
        public string StreetAddress2 { get; set; }

        public bool ZipPostalCodeEnabled { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.Fields.ZipPostalCode")]
        [AllowHtml]
        public string ZipPostalCode { get; set; }

        public bool CityEnabled { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.Fields.City")]
        [AllowHtml]
        public string City { get; set; }

        public bool CountryEnabled { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.Fields.Country")]
        public int CountryId { get; set; }
        public IList<SelectListItem> AvailableCountries { get; set; }

        public bool DepartmentEnabled { get; set; }
        [SSGResourceDisplayName("Department name")]
        public int? DepartmentId { get; set; }
        public IList<SelectListItem> AvailableDepartments { get; set; }

        public bool StateProvinceEnabled { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.Fields.StateProvince")]
        public int StateProvinceId { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }

        public bool PhoneEnabled { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.Fields.Phone")]
        [AllowHtml]
        public string Phone { get; set; }

        public bool FaxEnabled { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.Fields.Fax")]
        [AllowHtml]
        public string Fax { get; set; }

        [SSGResourceDisplayName("Admin.Users.Users.Fields.EmployeeID")]
        [AllowHtml]
        public string EmployeeId { get; set; }





        [SSGResourceDisplayName("Admin.Users.Users.Fields.AdminComment")]
        [AllowHtml]
        public string AdminComment { get; set; }
        
        [SSGResourceDisplayName("Admin.Users.Users.Fields.Active")]
        public bool Active { get; set; }



        //time zone
        [SSGResourceDisplayName("Admin.Users.Users.Fields.TimeZoneId")]
        [AllowHtml]
        public string TimeZoneId { get; set; }

        public bool AllowUsersToSetTimeZone { get; set; }

        public IList<SelectListItem> AvailableTimeZones { get; set; }




        //registration date
        [SSGResourceDisplayName("Admin.Users.Users.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [SSGResourceDisplayName("Admin.Users.Users.Fields.LastActivityDate")]
        public DateTime LastActivityDate { get; set; }

        //IP adderss
        [SSGResourceDisplayName("Admin.Users.Users.Fields.IPAddress")]
        public string LastIpAddress { get; set; }


        [SSGResourceDisplayName("Admin.Users.Users.Fields.LastVisitedPage")]
        public string LastVisitedPage { get; set; }


        //user roles
        [SSGResourceDisplayName("Admin.Users.Users.Fields.UserRoles")]
        public string UserRoleNames { get; set; }
        public List<UserRoleModel> AvailableUserRoles { get; set; }
        public int[] SelectedUserRoleIds { get; set; }
        public bool AllowManagingUserRoles { get; set; }

        //buyer departments
        [SSGResourceDisplayName("Buyer Departments")]
        public string BuyerDepartmentNames { get; set; }
        public List<Department> AvailableBuyerDepartments { get; set; }
        public int[] SelectedBuyerDepartmentIds { get; set; }
        public bool AllowManageingBuyerDepartments { get; set; }

  
        //send email model
        public SendEmailModel SendEmail { get; set; }
        //send PM model
        public SendPmModel SendPm { get; set; }

        [SSGResourceDisplayName("Admin.Users.Users.AssociatedExternalAuth")]
        public IList<AssociatedExternalAuthModel> AssociatedExternalAuthRecords { get; set; }

        
        #region Nested classes

        public partial class AssociatedExternalAuthModel : BaseSSGEntityModel
        {
            [SSGResourceDisplayName("Admin.Users.Users.AssociatedExternalAuth.Fields.Email")]
            public string Email { get; set; }

            [SSGResourceDisplayName("Admin.Users.Users.AssociatedExternalAuth.Fields.ExternalIdentifier")]
            public string ExternalIdentifier { get; set; }

            [SSGResourceDisplayName("Admin.Users.Users.AssociatedExternalAuth.Fields.AuthMethodName")]
            public string AuthMethodName { get; set; }
        }

        public partial class SendEmailModel : BaseSSGModel
        {
            [SSGResourceDisplayName("Admin.Users.Users.SendEmail.Subject")]
            [AllowHtml]
            public string Subject { get; set; }

            [SSGResourceDisplayName("Admin.Users.Users.SendEmail.Body")]
            [AllowHtml]
            public string Body { get; set; }
        }

        public partial class SendPmModel : BaseSSGModel
        {
            [SSGResourceDisplayName("Admin.Users.Users.SendPM.Subject")]
            public string Subject { get; set; }

            [SSGResourceDisplayName("Admin.Users.Users.SendPM.Message")]
            public string Message { get; set; }
        }

        public partial class ActivityLogModel : BaseSSGEntityModel
        {
            [SSGResourceDisplayName("Admin.Users.Users.ActivityLog.ActivityLogType")]
            public string ActivityLogTypeName { get; set; }
            [SSGResourceDisplayName("Admin.Users.Users.ActivityLog.Comment")]
            public string Comment { get; set; }
            [SSGResourceDisplayName("Admin.Users.Users.ActivityLog.CreatedOn")]
            public DateTime CreatedOn { get; set; }
        }
        #endregion
    }
}