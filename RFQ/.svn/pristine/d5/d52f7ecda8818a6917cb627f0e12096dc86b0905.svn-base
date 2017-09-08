using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SSG.Core.Domain.Common;
using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;

namespace SSG.Admin.Models.Settings
{
    public partial class GeneralCommonSettingsModel : BaseSSGModel
    {
        public GeneralCommonSettingsModel()
        {
            SiteInformationSettings = new SiteInformationSettingsModel();
            SeoSettings = new SeoSettingsModel();
            SecuritySettings = new SecuritySettingsModel();
            PdfSettings = new PdfSettingsModel();
            LocalizationSettings = new LocalizationSettingsModel();
            FullTextSettings = new FullTextSettingsModel();
        }
        public SiteInformationSettingsModel SiteInformationSettings { get; set; }
        public SeoSettingsModel SeoSettings { get; set; }
        public SecuritySettingsModel SecuritySettings { get; set; }
        public PdfSettingsModel PdfSettings { get; set; }
        public LocalizationSettingsModel LocalizationSettings { get; set; }
        public FullTextSettingsModel FullTextSettings { get; set; }

        #region Nested classes

        public partial class SiteInformationSettingsModel
        {
            public SiteInformationSettingsModel()
            {
                this.AvailableSiteThemesForDesktops = new List<SelectListItem>();
                this.AvailableSiteThemesForMobileDevices = new List<SelectListItem>();
            }
            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SiteName")]
            [AllowHtml]
            public string SiteName { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SiteUrl")]
            [AllowHtml]
            public string SiteUrl { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.MobileDevicesSupported")]
            public bool MobileDevicesSupported { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SiteClosed")]
            public bool SiteClosed { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SiteClosedAllowForAdmins")]
            public bool SiteClosedAllowForAdmins { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DefaultSiteThemeForDesktops")]
            [AllowHtml]
            public string DefaultSiteThemeForDesktops { get; set; }
            public IList<SelectListItem> AvailableSiteThemesForDesktops { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DefaultSiteThemeForMobileDevices")]
            [AllowHtml]
            public string DefaultSiteThemeForMobileDevices { get; set; }
            public IList<SelectListItem> AvailableSiteThemesForMobileDevices { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.AllowUserToSelectTheme")]
            public bool AllowUserToSelectTheme { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayEuCookieLawWarning")]
            public bool DisplayEuCookieLawWarning { get; set; }
        }

        public partial class SeoSettingsModel
        {
            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PageTitleSeparator")]
            [AllowHtml]
            public string PageTitleSeparator { get; set; }
            
            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PageTitleSeoAdjustment")]
            public PageTitleSeoAdjustment PageTitleSeoAdjustment { get; set; }
            public SelectList PageTitleSeoAdjustmentValues { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DefaultTitle")]
            [AllowHtml]
            public string DefaultTitle { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DefaultMetaKeywords")]
            [AllowHtml]
            public string DefaultMetaKeywords { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DefaultMetaDescription")]
            [AllowHtml]
            public string DefaultMetaDescription { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.ConvertNonWesternChars")]
            public bool ConvertNonWesternChars { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CanonicalUrlsEnabled")]
            public bool CanonicalUrlsEnabled { get; set; }
        }

        public partial class SecuritySettingsModel
        {
            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.EncryptionKey")]
            [AllowHtml]
            public string EncryptionKey { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.AdminAreaAllowedIpAddresses")]
            [AllowHtml]
            public string AdminAreaAllowedIpAddresses { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.HideAdminMenuItemsBasedOnPermissions")]
            public bool HideAdminMenuItemsBasedOnPermissions { get; set; }




            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaEnabled")]
            public bool CaptchaEnabled { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaShowOnLoginPage")]
            public bool CaptchaShowOnLoginPage { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaShowOnRegistrationPage")]
            public bool CaptchaShowOnRegistrationPage { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaShowOnContactUsPage")]
            public bool CaptchaShowOnContactUsPage { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaShowOnNewsCommentPage")]
            public bool CaptchaShowOnNewsCommentPage { get; set; }
            
            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.reCaptchaPublicKey")]
            [AllowHtml]
            public string ReCaptchaPublicKey { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.reCaptchaPrivateKey")]
            [AllowHtml]
            public string ReCaptchaPrivateKey { get; set; }




            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.UseSSL")]
            public bool UseSsl { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SharedSSLUrl")]
            [AllowHtml]
            public string SharedSslUrl { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.NonSharedSSLUrl")]
            [AllowHtml]
            public string NonSharedSslUrl { get; set; }
        }

        public partial class PdfSettingsModel
        {
            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PdfEnabled")]
            public bool Enabled { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PdfLetterPageSizeEnabled")]
            public bool LetterPageSizeEnabled { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PdfLogo")]
            [UIHint("Picture")]
            public int LogoPictureId { get; set; }
        }

        public partial class LocalizationSettingsModel
        {
            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.UseImagesForLanguageSelection")]
            public bool UseImagesForLanguageSelection { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SeoFriendlyUrlsForLanguagesEnabled")]
            public bool SeoFriendlyUrlsForLanguagesEnabled { get; set; }
        }

        public partial class FullTextSettingsModel
        {
            public bool Supported { get; set; }

            public bool Enabled { get; set; }

            [SSGResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.FullTextSettings.SearchMode")]
            public FulltextSearchMode SearchMode { get; set; }
            public SelectList SearchModeValues { get; set; }
        }
        
        #endregion
    }
}