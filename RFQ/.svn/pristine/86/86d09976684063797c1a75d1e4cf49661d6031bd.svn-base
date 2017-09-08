using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using SSG.Admin.Models.Common;
using SSG.Admin.Models.Settings;
using SSG.Core;
using SSG.Core.Domain;
using SSG.Core.Domain.Common;
using SSG.Core.Domain.Directory;
using SSG.Core.Domain.Forums;
using SSG.Core.Domain.Localization;
using SSG.Core.Domain.Media;
using SSG.Core.Domain.News;
using SSG.Core.Domain.Security;
using SSG.Core.Domain.Users;
using SSG.Services.Common;
using SSG.Services.Configuration;
using SSG.Services.Directory;
using SSG.Services.Helpers;
using SSG.Services.Localization;
using SSG.Services.Logging;
using SSG.Services.Media;
using SSG.Services.Security;
using SSG.Services.Users;
using SSG.Web.Framework;
using SSG.Web.Framework.Controllers;
using SSG.Web.Framework.Localization;
using SSG.Web.Framework.Themes;
using SSG.Web.Framework.UI.Captcha;
using Telerik.Web.Mvc;

namespace SSG.Admin.Controllers
{
	[AdminAuthorize]
    public partial class SettingController : BaseSSGController
	{
		#region Fields

        private readonly ISettingService _settingService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IAddressService _addressService;
        private readonly ICurrencyService _currencyService;
        private readonly IPictureService _pictureService;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IEncryptionService _encryptionService;
        private readonly IThemeProvider _themeProvider;
        private readonly IUserService _userService;
        private readonly IUserActivityService _userActivityService;
        private readonly IPermissionService _permissionService;
        private readonly IWebHelper _webHelper;
	    private readonly IFulltextService _fulltextService;


        private ForumSettings _forumSettings;
        private NewsSettings _newsSettings;
        private readonly CurrencySettings _currencySettings;
        private MediaSettings _mediaSettings;
        private UserSettings _userSettings;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly SiteInformationSettings _siteInformationSettings;
        private readonly SeoSettings _seoSettings;
        private readonly SecuritySettings _securitySettings;
        private readonly PdfSettings _pdfSettings;
        private readonly LocalizationSettings _localizationSettings;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly CaptchaSettings _captchaSettings;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
	    private readonly CommonSettings _commonSettings;

		#endregion

		#region Constructors

        public SettingController(ISettingService settingService,
            ICountryService countryService, IStateProvinceService stateProvinceService,
            IAddressService addressService, 
            ICurrencyService currencyService, IPictureService pictureService, 
            ILocalizationService localizationService, IDateTimeHelper dateTimeHelper,
            IEncryptionService encryptionService,
            IThemeProvider themeProvider, IUserService userService, 
            IUserActivityService userActivityService, IPermissionService permissionService,
            IWebHelper webHelper, IFulltextService fulltextService,
            ForumSettings forumSettings, NewsSettings newsSettings,
            CurrencySettings currencySettings, MediaSettings mediaSettings,
            UserSettings userSettings,
            DateTimeSettings dateTimeSettings, SiteInformationSettings siteInformationSettings,
            SeoSettings seoSettings,SecuritySettings securitySettings, PdfSettings pdfSettings,
            LocalizationSettings localizationSettings, AdminAreaSettings adminAreaSettings,
            CaptchaSettings captchaSettings, ExternalAuthenticationSettings externalAuthenticationSettings,
            CommonSettings commonSettings)
        {
            this._settingService = settingService;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._addressService = addressService;
            this._currencyService = currencyService;
            this._pictureService = pictureService;
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
            this._encryptionService = encryptionService;
            this._themeProvider = themeProvider;
            this._userService = userService;
            this._userActivityService = userActivityService;
            this._permissionService = permissionService;
            this._webHelper = webHelper;
            this._fulltextService = fulltextService;

            this._forumSettings = forumSettings;
            this._newsSettings = newsSettings;
            this._currencySettings = currencySettings;
            this._mediaSettings = mediaSettings;
            this._userSettings = userSettings;
            this._dateTimeSettings = dateTimeSettings;
            this._siteInformationSettings = siteInformationSettings;
            this._seoSettings = seoSettings;
            this._securitySettings = securitySettings;
            this._pdfSettings = pdfSettings;
            this._localizationSettings = localizationSettings;
            this._adminAreaSettings = adminAreaSettings;
            this._captchaSettings = captchaSettings;
            this._externalAuthenticationSettings = externalAuthenticationSettings;
            this._commonSettings = commonSettings;
        }

		#endregion 
        
        #region Methods

        public ActionResult Forum()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var model = _forumSettings.ToModel();
            model.ForumEditorValues = _forumSettings.ForumEditor.ToSelectList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Forum(ForumSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            _forumSettings = model.ToEntity(_forumSettings);
            _settingService.SaveSetting(_forumSettings);

            //activity log
            _userActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));
            return RedirectToAction("Forum");
        }




        public ActionResult News()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var model = _newsSettings.ToModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult News(NewsSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            _newsSettings = model.ToEntity(_newsSettings);
            _settingService.SaveSetting(_newsSettings);

            //activity log
            _userActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));
            return RedirectToAction("News");
        }


        public ActionResult Media()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var model = _mediaSettings.ToModel();
            model.PicturesStoredIntoDatabase = _pictureService.SiteInDb;
            return View(model);
        }
        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult Media(MediaSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            _mediaSettings = model.ToEntity(_mediaSettings);
            _settingService.SaveSetting(_mediaSettings);

            //activity log
            _userActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));
            return RedirectToAction("Media");
        }
        [HttpPost, ActionName("Media")]
        [FormValueRequired("change-picture-storage")]
        public ActionResult ChangePictureStorage()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            _pictureService.SiteInDb = !_pictureService.SiteInDb;

            //activity log
            _userActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));
            return RedirectToAction("Media");
        }



        public ActionResult UserUser()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //merge settings
            var model = new UserUserSettingsModel();
            model.UserSettings = _userSettings.ToModel();

            model.DateTimeSettings.AllowUsersToSetTimeZone = _dateTimeSettings.AllowUsersToSetTimeZone;
            model.DateTimeSettings.DefaultSiteTimeZoneId = _dateTimeHelper.DefaultSiteTimeZone.Id;
            foreach (TimeZoneInfo timeZone in _dateTimeHelper.GetSystemTimeZones())
            {
                model.DateTimeSettings.AvailableTimeZones.Add(new SelectListItem()
                    {
                        Text = timeZone.DisplayName,
                        Value = timeZone.Id,
                        Selected = timeZone.Id.Equals(_dateTimeHelper.DefaultSiteTimeZone.Id, StringComparison.InvariantCultureIgnoreCase)
                    });
            }

            model.ExternalAuthenticationSettings.AutoRegisterEnabled = _externalAuthenticationSettings.AutoRegisterEnabled;

            return View(model);
        }
        [HttpPost]
        public ActionResult UserUser(UserUserSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            _userSettings = model.UserSettings.ToEntity(_userSettings);
            _settingService.SaveSetting(_userSettings);
            
            _dateTimeSettings.DefaultSiteTimeZoneId = model.DateTimeSettings.DefaultSiteTimeZoneId;
            _dateTimeSettings.AllowUsersToSetTimeZone = model.DateTimeSettings.AllowUsersToSetTimeZone;
            _settingService.SaveSetting(_dateTimeSettings);

            _externalAuthenticationSettings.AutoRegisterEnabled = model.ExternalAuthenticationSettings.AutoRegisterEnabled;
            _settingService.SaveSetting(_externalAuthenticationSettings);

            //activity log
            _userActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));
            return RedirectToAction("UserUser");
        }






        public ActionResult GeneralCommon(string selectedTab)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //set page timeout to 5 minutes
            this.Server.ScriptTimeout = 300;

            //site information
            var model = new GeneralCommonSettingsModel();
            model.SiteInformationSettings.SiteName = _siteInformationSettings.SiteName;
            model.SiteInformationSettings.SiteUrl = _siteInformationSettings.SiteUrl;
            model.SiteInformationSettings.SiteClosed = _siteInformationSettings.SiteClosed;
            model.SiteInformationSettings.SiteClosedAllowForAdmins = _siteInformationSettings.SiteClosedAllowForAdmins;
            //desktop themes
            model.SiteInformationSettings.DefaultSiteThemeForDesktops = _siteInformationSettings.DefaultSiteThemeForDesktops;
            model.SiteInformationSettings.AvailableSiteThemesForDesktops = _themeProvider
                .GetThemeConfigurations()
                .Where(x => !x.MobileTheme)  //do not display themes for mobile devices
                .Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.ThemeTitle,
                        Value = x.ThemeName,
                        Selected = x.ThemeName.Equals(_siteInformationSettings.DefaultSiteThemeForDesktops, StringComparison.InvariantCultureIgnoreCase)
                    };
                })
                .ToList();
            model.SiteInformationSettings.AllowUserToSelectTheme = _siteInformationSettings.AllowUserToSelectTheme;
            model.SiteInformationSettings.MobileDevicesSupported = _siteInformationSettings.MobileDevicesSupported;
            //mobile device themes
            model.SiteInformationSettings.DefaultSiteThemeForMobileDevices = _siteInformationSettings.DefaultSiteThemeForMobileDevices;
            model.SiteInformationSettings.AvailableSiteThemesForMobileDevices = _themeProvider
                .GetThemeConfigurations()
                .Where(x => x.MobileTheme)  //do not display themes for desktops
                .Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.ThemeTitle,
                        Value = x.ThemeName,
                        Selected = x.ThemeName.Equals(_siteInformationSettings.DefaultSiteThemeForMobileDevices, StringComparison.InvariantCultureIgnoreCase)
                    };
                })
                .ToList();
            //EU Cookie law
            model.SiteInformationSettings.DisplayEuCookieLawWarning = _siteInformationSettings.DisplayEuCookieLawWarning;

            //seo settings
            model.SeoSettings.PageTitleSeparator = _seoSettings.PageTitleSeparator;
            model.SeoSettings.DefaultTitle = _seoSettings.DefaultTitle;
            model.SeoSettings.DefaultMetaKeywords = _seoSettings.DefaultMetaKeywords;
            model.SeoSettings.DefaultMetaDescription = _seoSettings.DefaultMetaDescription;
            model.SeoSettings.ConvertNonWesternChars = _seoSettings.ConvertNonWesternChars;
            model.SeoSettings.CanonicalUrlsEnabled = _seoSettings.CanonicalUrlsEnabled;
            model.SeoSettings.PageTitleSeoAdjustmentValues = _seoSettings.PageTitleSeoAdjustment.ToSelectList();
            
            //security settings
            model.SecuritySettings.EncryptionKey = _securitySettings.EncryptionKey;
            if (_securitySettings.AdminAreaAllowedIpAddresses!=null)
                for (int i=0;i<_securitySettings.AdminAreaAllowedIpAddresses.Count; i++)
                {
                    model.SecuritySettings.AdminAreaAllowedIpAddresses += _securitySettings.AdminAreaAllowedIpAddresses[i];
                    if (i != _securitySettings.AdminAreaAllowedIpAddresses.Count - 1)
                        model.SecuritySettings.AdminAreaAllowedIpAddresses += ",";
                }
            model.SecuritySettings.HideAdminMenuItemsBasedOnPermissions = _securitySettings.HideAdminMenuItemsBasedOnPermissions;
            model.SecuritySettings.CaptchaEnabled = _captchaSettings.Enabled;
            model.SecuritySettings.CaptchaShowOnLoginPage = _captchaSettings.ShowOnLoginPage;
            model.SecuritySettings.CaptchaShowOnRegistrationPage = _captchaSettings.ShowOnRegistrationPage;
            model.SecuritySettings.CaptchaShowOnContactUsPage = _captchaSettings.ShowOnContactUsPage;
            model.SecuritySettings.CaptchaShowOnNewsCommentPage = _captchaSettings.ShowOnNewsCommentPage;
            model.SecuritySettings.ReCaptchaPublicKey = _captchaSettings.ReCaptchaPublicKey;
            model.SecuritySettings.ReCaptchaPrivateKey = _captchaSettings.ReCaptchaPrivateKey;

            bool useSsl = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["UseSSL"]) &&
                          Convert.ToBoolean(ConfigurationManager.AppSettings["UseSSL"]);
            string sharedSslUrl = ConfigurationManager.AppSettings["SharedSSLUrl"];
            string nonSharedSslUrl = ConfigurationManager.AppSettings["NonSharedSSLUrl"];
            model.SecuritySettings.UseSsl = useSsl;
            model.SecuritySettings.SharedSslUrl = sharedSslUrl;
            model.SecuritySettings.NonSharedSslUrl = nonSharedSslUrl;

            //PDF settings
            model.PdfSettings.Enabled = _pdfSettings.Enabled;
            model.PdfSettings.LetterPageSizeEnabled = _pdfSettings.LetterPageSizeEnabled;
            model.PdfSettings.LogoPictureId = _pdfSettings.LogoPictureId;

            //localization
            model.LocalizationSettings.UseImagesForLanguageSelection = _localizationSettings.UseImagesForLanguageSelection;
            model.LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled = _localizationSettings.SeoFriendlyUrlsForLanguagesEnabled;

            //full-text support
            model.FullTextSettings.Supported = _fulltextService.IsFullTextSupported();
            model.FullTextSettings.Enabled = _commonSettings.UseFullTextSearch;
            model.FullTextSettings.SearchModeValues = _commonSettings.FullTextMode.ToSelectList();


            ViewData["selectedTab"] = selectedTab;
            return View(model);
        }
        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult GeneralCommon(GeneralCommonSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //site information
            _siteInformationSettings.SiteName = model.SiteInformationSettings.SiteName;
            if (model.SiteInformationSettings.SiteUrl == null)
                model.SiteInformationSettings.SiteUrl = "";
            _siteInformationSettings.SiteUrl = model.SiteInformationSettings.SiteUrl;
            //ensure we have "/" at the end
            if (!_siteInformationSettings.SiteUrl.EndsWith("/"))
                _siteInformationSettings.SiteUrl += "/";
            _siteInformationSettings.SiteClosed = model.SiteInformationSettings.SiteClosed;
            _siteInformationSettings.SiteClosedAllowForAdmins = model.SiteInformationSettings.SiteClosedAllowForAdmins;
            _siteInformationSettings.DefaultSiteThemeForDesktops = model.SiteInformationSettings.DefaultSiteThemeForDesktops;
            _siteInformationSettings.AllowUserToSelectTheme = model.SiteInformationSettings.AllowUserToSelectTheme;
            //site whether MobileDevicesSupported setting has been changed (requires application restart)
            bool mobileDevicesSupportedChanged = _siteInformationSettings.MobileDevicesSupported !=
                                                 model.SiteInformationSettings.MobileDevicesSupported;
            _siteInformationSettings.MobileDevicesSupported = model.SiteInformationSettings.MobileDevicesSupported;
            _siteInformationSettings.DefaultSiteThemeForMobileDevices = model.SiteInformationSettings.DefaultSiteThemeForMobileDevices;
            //EU Cookie law
            _siteInformationSettings.DisplayEuCookieLawWarning = model.SiteInformationSettings.DisplayEuCookieLawWarning;
            _settingService.SaveSetting(_siteInformationSettings);



            //seo settings
            _seoSettings.PageTitleSeparator = model.SeoSettings.PageTitleSeparator;
            _seoSettings.DefaultTitle = model.SeoSettings.DefaultTitle;
            _seoSettings.DefaultMetaKeywords = model.SeoSettings.DefaultMetaKeywords;
            _seoSettings.DefaultMetaDescription = model.SeoSettings.DefaultMetaDescription;
            _seoSettings.ConvertNonWesternChars = model.SeoSettings.ConvertNonWesternChars;
            _seoSettings.CanonicalUrlsEnabled = model.SeoSettings.CanonicalUrlsEnabled;
            _seoSettings.PageTitleSeoAdjustment = model.SeoSettings.PageTitleSeoAdjustment;
            _settingService.SaveSetting(_seoSettings);



            //security settings
            if (_securitySettings.AdminAreaAllowedIpAddresses == null)
                _securitySettings.AdminAreaAllowedIpAddresses = new List<string>();
            _securitySettings.AdminAreaAllowedIpAddresses.Clear();
            if (!String.IsNullOrEmpty(model.SecuritySettings.AdminAreaAllowedIpAddresses))
                foreach (string s in model.SecuritySettings.AdminAreaAllowedIpAddresses.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    if (!String.IsNullOrWhiteSpace(s))
                        _securitySettings.AdminAreaAllowedIpAddresses.Add(s.Trim());
            _securitySettings.HideAdminMenuItemsBasedOnPermissions = model.SecuritySettings.HideAdminMenuItemsBasedOnPermissions;
            _settingService.SaveSetting(_securitySettings);
            _captchaSettings.Enabled = model.SecuritySettings.CaptchaEnabled;
            _captchaSettings.ShowOnLoginPage = model.SecuritySettings.CaptchaShowOnLoginPage;
            _captchaSettings.ShowOnRegistrationPage = model.SecuritySettings.CaptchaShowOnRegistrationPage;
            _captchaSettings.ShowOnContactUsPage = model.SecuritySettings.CaptchaShowOnContactUsPage;
            _captchaSettings.ShowOnNewsCommentPage = model.SecuritySettings.CaptchaShowOnNewsCommentPage;
            _captchaSettings.ReCaptchaPublicKey = model.SecuritySettings.ReCaptchaPublicKey;
            _captchaSettings.ReCaptchaPrivateKey = model.SecuritySettings.ReCaptchaPrivateKey;
            _settingService.SaveSetting(_captchaSettings);
            if (_captchaSettings.Enabled &&
                (String.IsNullOrWhiteSpace(_captchaSettings.ReCaptchaPublicKey) || String.IsNullOrWhiteSpace(_captchaSettings.ReCaptchaPrivateKey)))
            {
                //captcha is enabled but the keys are not entered
                ErrorNotification("Captcha is enabled but the appropriate keys are not entered");
            }
            //save SSL settings
            try
            {
                if (AppDomain.CurrentDomain.IsFullyTrusted)
                {
                    //full trust
                    bool useSsl = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["UseSSL"]) &&
                                  Convert.ToBoolean(ConfigurationManager.AppSettings["UseSSL"]);
                    string sharedSslUrl = ConfigurationManager.AppSettings["SharedSSLUrl"];
                    string nonSharedSslUrl = ConfigurationManager.AppSettings["NonSharedSSLUrl"];
                    //use this field to prevent web.config saving if not changes are done (can cause application restart)
                    bool sslChanged = false;

                    var config = WebConfigurationManager.OpenWebConfiguration("~");
                    if (useSsl != model.SecuritySettings.UseSsl)
                    {
                        config.AppSettings.Settings["UseSSL"].Value = model.SecuritySettings.UseSsl.ToString();
                        sslChanged = true;
                    }
                    if (model.SecuritySettings.SharedSslUrl == null)
                        model.SecuritySettings.SharedSslUrl = "";
                    if (sharedSslUrl != model.SecuritySettings.SharedSslUrl)
                    {
                        config.AppSettings.Settings["SharedSSLUrl"].Value = model.SecuritySettings.SharedSslUrl;
                        sslChanged = true;
                    }

                    if (model.SecuritySettings.NonSharedSslUrl == null)
                        model.SecuritySettings.NonSharedSslUrl = "";
                    if (nonSharedSslUrl != model.SecuritySettings.NonSharedSslUrl)
                    {
                        config.AppSettings.Settings["NonSharedSSLUrl"].Value = model.SecuritySettings.NonSharedSslUrl;
                        sslChanged = true;
                    }
                    if (sslChanged)
                        config.Save(ConfigurationSaveMode.Modified);
                }
                else
                {
                    //medium trust
                    ErrorNotification("SSL settings cannot be saved in medium trust. Manually update web.config file.");

                }
            }
            catch (Exception exc)
            {
                ErrorNotification("SSL settings cannot be saved in medium trust. Manually update web.config file. " + exc.Message);
            }


            //PDF settings
            _pdfSettings.Enabled = model.PdfSettings.Enabled;
            _pdfSettings.LetterPageSizeEnabled = model.PdfSettings.LetterPageSizeEnabled;
            _pdfSettings.LogoPictureId = model.PdfSettings.LogoPictureId;
            _settingService.SaveSetting(_pdfSettings);


            //localization settings
            _localizationSettings.UseImagesForLanguageSelection = model.LocalizationSettings.UseImagesForLanguageSelection;
            if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled != model.LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                _localizationSettings.SeoFriendlyUrlsForLanguagesEnabled = model.LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled;
                //clear cached values of routes
                System.Web.Routing.RouteTable.Routes.ClearSeoFriendlyUrlsCachedValueForRoutes();
            }
            _settingService.SaveSetting(_localizationSettings);

            //full-text
            _commonSettings.FullTextMode = model.FullTextSettings.SearchMode;
            _settingService.SaveSetting(_commonSettings);

            //activity log
            _userActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            if (mobileDevicesSupportedChanged)
            {
                //MobileDevicesSupported setting has been changed
                //restart application
                _webHelper.RestartAppDomain();
            }

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));
            return RedirectToAction("GeneralCommon");
        }
        [HttpPost, ActionName("GeneralCommon")]
        [FormValueRequired("changeencryptionkey")]
        public ActionResult ChangeEnryptionKey(GeneralCommonSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //set page timeout to 5 minutes
            this.Server.ScriptTimeout = 300;

            try
            {
                if (model.SecuritySettings.EncryptionKey == null)
                    model.SecuritySettings.EncryptionKey = "";

                model.SecuritySettings.EncryptionKey = model.SecuritySettings.EncryptionKey.Trim();

                var newEncryptionPrivateKey = model.SecuritySettings.EncryptionKey;
                if (String.IsNullOrEmpty(newEncryptionPrivateKey) || newEncryptionPrivateKey.Length != 16)
                    throw new SSGException(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.TooShort"));

                string oldEncryptionPrivateKey = _securitySettings.EncryptionKey;
                if (oldEncryptionPrivateKey == newEncryptionPrivateKey)
                    throw new SSGException(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.TheSame"));

                //update user information
                //optimization - load only users with PasswordFormat.Encrypted
                var users = _userService.GetAllUsersByPasswordFormat(PasswordFormat.Encrypted);
                foreach (var user in users)
                {
                    string decryptedPassword = _encryptionService.DecryptText(user.Password, oldEncryptionPrivateKey);
                    string encryptedPassword = _encryptionService.EncryptText(decryptedPassword, newEncryptionPrivateKey);

                    user.Password = encryptedPassword;
                    _userService.UpdateUser(user);
                }

                _securitySettings.EncryptionKey = newEncryptionPrivateKey;
                _settingService.SaveSetting(_securitySettings);
                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.Changed"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }
            return RedirectToAction("GeneralCommon", new { selectedTab = "security" });
        }
        [HttpPost, ActionName("GeneralCommon")]
        [FormValueRequired("togglefulltext")]
        public ActionResult ToggleFullText(GeneralCommonSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            try
            {
                if (! _fulltextService.IsFullTextSupported())
                    throw new SSGException(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.FullTextSettings.NotSupported"));

                if (_commonSettings.UseFullTextSearch)
                {
                    _fulltextService.DisableFullText();

                    _commonSettings.UseFullTextSearch = false;
                    _settingService.SaveSetting(_commonSettings);

                    SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.FullTextSettings.Disabled"));
                }
                else
                {
                    _fulltextService.EnableFullText();

                    _commonSettings.UseFullTextSearch = true;
                    _settingService.SaveSetting(_commonSettings);

                    SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.FullTextSettings.Enabled"));
                }
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }
            return RedirectToAction("GeneralCommon", new { selectedTab = "fulltext" });
        }




        //all settings
        public ActionResult AllSettings()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();
            
            var settings = _settingService
                .GetAllSettings()
                .OrderBy(x => x.Key)
                .ToList();
            var model = new GridModel<SettingModel>
            {
                Data = settings.Take(_adminAreaSettings.GridPageSize).Select(x => 
                {
                    return new SettingModel()
                    {
                        Id = x.Value.Key,
                        Name = x.Key,
                        Value = x.Value.Value
                    };
                }),
                Total = settings.Count
            };
            return View(model);
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult AllSettings(GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var settings = _settingService
                .GetAllSettings()
                .OrderBy(x => x.Key)
                .Select(x => new SettingModel()
                    {
                        Id = x.Value.Key,
                        Name = x.Key,
                        Value = x.Value.Value
                    })
                .ForCommand(command)
                .ToList();
            
            var model = new GridModel<SettingModel>
            {
                Data = settings.PagedForCommand(command),
                Total = settings.Count
            };
            return new JsonResult
            {
                Data = model
            };
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult SettingUpdate(SettingModel model, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            if (model.Name != null)
                model.Name = model.Name.Trim();
            if (model.Value != null)
                model.Value = model.Value.Trim();

            if (!ModelState.IsValid)
            {
                //display the first model error
                var modelStateErrors = this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                return Content(modelStateErrors.FirstOrDefault());
            }

            var setting = _settingService.GetSettingById(model.Id);
            if (setting.Name != model.Name)
                _settingService.DeleteSetting(setting);

            _settingService.SetSetting(model.Name, model.Value);

            //activity log
            _userActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            return AllSettings(command);
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult SettingAdd([Bind(Exclude = "Id")] SettingModel model, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            if (model.Name != null)
                model.Name = model.Name.Trim();
            if (model.Value != null)
                model.Value = model.Value.Trim();

            if (!ModelState.IsValid)
            {
                //display the first model error
                var modelStateErrors = this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                return Content(modelStateErrors.FirstOrDefault());
            }

            _settingService.SetSetting(model.Name, model.Value);

            //activity log
            _userActivityService.InsertActivity("AddNewSetting", _localizationService.GetResource("ActivityLog.AddNewSetting"), model.Name);

            return AllSettings(command);
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult SettingDelete(int id, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var setting = _settingService.GetSettingById(id);
            if (setting == null)
                throw new ArgumentException("No setting found with the specified id");
            _settingService.DeleteSetting(setting);

            //activity log
            _userActivityService.InsertActivity("DeleteSetting", _localizationService.GetResource("ActivityLog.DeleteSetting"), setting.Name);

            return AllSettings(command);
        }

        #endregion
    }
}
