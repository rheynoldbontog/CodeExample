using System.Collections.Generic;
using System.Web.Mvc;
using SSG.Web.Framework;

namespace SSG.Admin.Models.Settings
{
    public partial class UserUserSettingsModel
    {
        public UserUserSettingsModel()
        {
            UserSettings = new UserSettingsModel();
            DateTimeSettings = new DateTimeSettingsModel();
            ExternalAuthenticationSettings = new ExternalAuthenticationSettingsModel();
        }
        public UserSettingsModel UserSettings { get; set; }
        public DateTimeSettingsModel DateTimeSettings { get; set; }
        public ExternalAuthenticationSettingsModel ExternalAuthenticationSettings { get; set; }

        #region Nested classes
        
        public partial class UserSettingsModel
        {
            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.UsernamesEnabled")]
            public bool UsernamesEnabled { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowUsersToChangeUsernames")]
            public bool AllowUsersToChangeUsernames { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.CheckUsernameAvailabilityEnabled")]
            public bool CheckUsernameAvailabilityEnabled { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.UserRegistrationType")]
            public int UserRegistrationType { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowUsersToUploadAvatars")]
            public bool AllowUsersToUploadAvatars { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.DefaultAvatarEnabled")]
            public bool DefaultAvatarEnabled { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.ShowUsersLocation")]
            public bool ShowUsersLocation { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.ShowUsersJoinDate")]
            public bool ShowUsersJoinDate { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowViewingProfiles")]
            public bool AllowViewingProfiles { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.NotifyNewUserRegistration")]
            public bool NotifyNewUserRegistration { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.UserNameFormat")]
            public int UserNameFormat { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.DefaultPasswordFormat")]
            public int DefaultPasswordFormat { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.NewsletterEnabled")]
            public bool NewsletterEnabled { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.HideNewsletterBlock")]
            public bool HideNewsletterBlock { get; set; }




            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.GenderEnabled")]
            public bool GenderEnabled { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.DateOfBirthEnabled")]
            public bool DateOfBirthEnabled { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.CompanyEnabled")]
            public bool CompanyEnabled { get; set; }
            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.CompanyRequired")]
            public bool CompanyRequired { get; set; }

            [SSGResourceDisplayName("'Department' enabled")]
            public bool DepartmentEnabled { get; set; }
            [SSGResourceDisplayName("'Department' required")]
            public bool DepartmentRequired { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.StreetAddressEnabled")]
            public bool StreetAddressEnabled { get; set; }
            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.StreetAddressRequired")]
            public bool StreetAddressRequired { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.StreetAddress2Enabled")]
            public bool StreetAddress2Enabled { get; set; }
            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.StreetAddress2Required")]
            public bool StreetAddress2Required { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.ZipPostalCodeEnabled")]
            public bool ZipPostalCodeEnabled { get; set; }
            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.ZipPostalCodeRequired")]
            public bool ZipPostalCodeRequired { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.CityEnabled")]
            public bool CityEnabled { get; set; }
            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.CityRequired")]
            public bool CityRequired { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.CountryEnabled")]
            public bool CountryEnabled { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.StateProvinceEnabled")]
            public bool StateProvinceEnabled { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.PhoneEnabled")]
            public bool PhoneEnabled { get; set; }
            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.PhoneRequired")]
            public bool PhoneRequired { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.FaxEnabled")]
            public bool FaxEnabled { get; set; }
            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.FaxRequired")]
            public bool FaxRequired { get; set; }
        }
        
        public partial class DateTimeSettingsModel
        {
            public DateTimeSettingsModel()
            {
                AvailableTimeZones = new List<SelectListItem>();
            }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowUsersToSetTimeZone")]
            public bool AllowUsersToSetTimeZone { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.DefaultSiteTimeZone")]
            public string DefaultSiteTimeZoneId { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.DefaultSiteTimeZone")]
            public IList<SelectListItem> AvailableTimeZones { get; set; }
        }

        public partial class ExternalAuthenticationSettingsModel
        {
            [SSGResourceDisplayName("Admin.Configuration.Settings.UserUser.ExternalAuthenticationAutoRegisterEnabled")]
            public bool AutoRegisterEnabled { get; set; }
        }
        #endregion
    }
}