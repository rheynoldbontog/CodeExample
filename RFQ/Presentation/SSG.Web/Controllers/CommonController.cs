using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSG.Core;
using SSG.Core.Caching;
using SSG.Core.Domain;
using SSG.Core.Domain.Common;
using SSG.Core.Domain.Forums;
using SSG.Core.Domain.Localization;
using SSG.Core.Domain.Messages;
using SSG.Core.Domain.Users;
using SSG.Services.Common;
using SSG.Services.Directory;
using SSG.Services.Forums;
using SSG.Services.Localization;
using SSG.Services.Messages;
using SSG.Services.Security;
using SSG.Services.Seo;
using SSG.Services.Topics;
using SSG.Services.Users;
using SSG.Web.Extensions;
using SSG.Web.Framework.Localization;
using SSG.Web.Framework.Security;
using SSG.Web.Framework.Themes;
using SSG.Web.Framework.UI.Captcha;
using SSG.Web.Infrastructure.Cache;
using SSG.Web.Models.Common;

namespace SSG.Web.Controllers
{
    public partial class CommonController : BaseSSGController
    {
        #region Fields

        private readonly ITopicService _topicService;
        private readonly ILanguageService _languageService;
        private readonly ICurrencyService _currencyService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly ISitemapGenerator _sitemapGenerator;
        private readonly IThemeContext _themeContext;
        private readonly IThemeProvider _themeProvider;
        private readonly IForumService _forumservice;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWebHelper _webHelper;
        private readonly IPermissionService _permissionService;
        private readonly IMobileDeviceHelper _mobileDeviceHelper;
        private readonly HttpContextBase _httpContext;
        private readonly ICacheManager _cacheManager;

        private readonly UserSettings _userSettings;
        private readonly SiteInformationSettings _siteInformationSettings;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly CommonSettings _commonSettings;
        private readonly ForumSettings _forumSettings;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CaptchaSettings _captchaSettings;

        #endregion

        #region Constructors

        public CommonController(ITopicService topicService,
            ILanguageService languageService,
            ICurrencyService currencyService, ILocalizationService localizationService,
            IWorkContext workContext,
            IQueuedEmailService queuedEmailService, IEmailAccountService emailAccountService,
            ISitemapGenerator sitemapGenerator, IThemeContext themeContext,
            IThemeProvider themeProvider, IForumService forumService,
            IGenericAttributeService genericAttributeService, IWebHelper webHelper,
            IPermissionService permissionService, IMobileDeviceHelper mobileDeviceHelper,
            HttpContextBase httpContext, ICacheManager cacheManager,
            UserSettings userSettings, 
            SiteInformationSettings siteInformationSettings, EmailAccountSettings emailAccountSettings,
            CommonSettings commonSettings, ForumSettings forumSettings,
            LocalizationSettings localizationSettings, CaptchaSettings captchaSettings)
        {
            this._topicService = topicService;
            this._languageService = languageService;
            this._currencyService = currencyService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._queuedEmailService = queuedEmailService;
            this._emailAccountService = emailAccountService;
            this._sitemapGenerator = sitemapGenerator;
            this._themeContext = themeContext;
            this._themeProvider = themeProvider;
            this._forumservice = forumService;
            this._genericAttributeService = genericAttributeService;
            this._webHelper = webHelper;
            this._permissionService = permissionService;
            this._mobileDeviceHelper = mobileDeviceHelper;
            this._httpContext = httpContext;
            this._cacheManager = cacheManager;

            this._userSettings = userSettings;
            this._siteInformationSettings = siteInformationSettings;
            this._emailAccountSettings = emailAccountSettings;
            this._commonSettings = commonSettings;
            this._forumSettings = forumSettings;
            this._localizationSettings = localizationSettings;
            this._captchaSettings = captchaSettings;
        }

        #endregion

        #region Utilities

        [NonAction]
        protected LanguageSelectorModel PrepareLanguageSelectorModel()
        {
            var availableLanguages = _cacheManager.Get(ModelCacheEventConsumer.AVAILABLE_LANGUAGES_MODEL_KEY, () =>
            {
                var result = _languageService
                    .GetAllLanguages()
                    .Select(x => x.ToModel())
                    .ToList();
                return result;
            });

            var model = new LanguageSelectorModel()
            {
                CurrentLanguage = _workContext.WorkingLanguage.ToModel(),
                AvailableLanguages = availableLanguages,
                UseImages = _localizationSettings.UseImagesForLanguageSelection
            };
            return model;
        }

        [NonAction]
        protected CurrencySelectorModel PrepareCurrencySelectorModel()
        {
            var availableCurrencies = _cacheManager.Get(string.Format(ModelCacheEventConsumer.AVAILABLE_CURRENCIES_MODEL_KEY, _workContext.WorkingLanguage.Id), () =>
            {
                var result = _currencyService
                    .GetAllCurrencies()
                    .Select(x => x.ToModel())
                    .ToList();
                return result;
            });

            var model = new CurrencySelectorModel()
            {
                CurrentCurrency = _workContext.WorkingCurrency.ToModel(),
                AvailableCurrencies = availableCurrencies
            };
            return model;
        }

        [NonAction]
        protected int GetUnreadPrivateMessages()
        {
            var result = 0;
            var user = _workContext.CurrentUser;
            if (_forumSettings.AllowPrivateMessages && !user.IsGuest())
            {
                var privateMessages = _forumservice.GetAllPrivateMessages(0, user.Id, false, null, false, string.Empty, 0, 1);

                if (privateMessages.TotalCount > 0)
                {
                    result = privateMessages.TotalCount;
                }
            }

            return result;
        }

        #endregion

        #region Methods

        //language
        [ChildActionOnly]
        public virtual ActionResult LanguageSelector()
        {
            var model = PrepareLanguageSelectorModel();
            return PartialView(model);
        }
        public virtual ActionResult SetLanguage(int langid)
        {
            var language = _languageService.GetLanguageById(langid);
            if (language != null && language.Published)
            {
                _workContext.WorkingLanguage = language;
            }


            if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                string applicationPath = HttpContext.Request.ApplicationPath;
                if (HttpContext.Request.UrlReferrer != null)
                {
                    string redirectUrl = HttpContext.Request.UrlReferrer.PathAndQuery;
                    if (redirectUrl.IsLocalizedUrl(applicationPath, true))
                    {
                        //already localized URL
                        redirectUrl = redirectUrl.RemoveLocalizedPathFromRawUrl(applicationPath);
                    }
                    redirectUrl = redirectUrl.AddLocalizedPathToRawUrl(applicationPath, _workContext.WorkingLanguage);
                    return Redirect(redirectUrl);
                }
                else
                {
                    string redirectUrl = Url.RouteUrl("HomePage");
                    redirectUrl = redirectUrl.AddLocalizedPathToRawUrl(applicationPath, _workContext.WorkingLanguage);
                    return Redirect(redirectUrl);
                }
            }
            else
            {
                //TODO: URL referrer is null in IE 8. Fix it
                if (HttpContext.Request.UrlReferrer != null)
                {
                    return Redirect(HttpContext.Request.UrlReferrer.PathAndQuery);
                }
                else
                {
                    return RedirectToRoute("HomePage");
                }
            }
        }

        //currency
        [ChildActionOnly]
        public virtual ActionResult CurrencySelector()
        {
            var model = PrepareCurrencySelectorModel();
            return PartialView(model);
        }
        public virtual ActionResult CurrencySelected(int userCurrency)
        {
            var currency = _currencyService.GetCurrencyById(userCurrency);
            if (currency != null)
                _workContext.WorkingCurrency = currency;

            //TODO: URL referrer is null in IE 8. Fix it
            if (HttpContext.Request.UrlReferrer != null)
            {
                return Redirect(HttpContext.Request.UrlReferrer.PathAndQuery);
            }
            else
            {
                return RedirectToRoute("HomePage");
            }
        }

        //Configuration page (used on mobile devices)
        [ChildActionOnly]
        public virtual ActionResult ConfigButton()
        {
            var langModel = PrepareLanguageSelectorModel();
            var currModel = PrepareCurrencySelectorModel();
            //should we display the button?
            if (langModel.AvailableLanguages.Count > 1 ||
                currModel.AvailableCurrencies.Count > 1)
                return PartialView();
            else
                return Content("");
        }

        public virtual ActionResult Config()
        {
            return View();
        }
        
        //footer
        [ChildActionOnly]
        public virtual ActionResult JavaScriptDisabledWarning()
        {
            if (!_commonSettings.DisplayJavaScriptDisabledWarning)
                return Content("");

            return PartialView();
        }

        //header links
        [ChildActionOnly]
        public virtual ActionResult HeaderLinks()
        {
            var user = _workContext.CurrentUser;

            var unreadMessageCount = GetUnreadPrivateMessages();
            var unreadMessage = string.Empty;
            var alertMessage = string.Empty;
            if (unreadMessageCount > 0)
            {
                unreadMessage = string.Format(_localizationService.GetResource("PrivateMessages.TotalUnread"), unreadMessageCount);

                //notifications here
                if (_forumSettings.ShowAlertForPM && 
                    !user.GetAttribute<bool>(SystemUserAttributeNames.NotifiedAboutNewPrivateMessages))
                {
                    _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.NotifiedAboutNewPrivateMessages, true);
                    alertMessage = string.Format(_localizationService.GetResource("PrivateMessages.YouHaveUnreadPM"), unreadMessageCount);
                }
            }

            var model = new HeaderLinksModel()
            {
                IsAuthenticated = user.IsRegistered(),
                UserEmailUsername = user.IsRegistered() ? (_userSettings.UsernamesEnabled ? user.Username : user.Email) : "",
                IsUserImpersonated = _workContext.OriginalUserIfImpersonated != null,
                DisplayAdminLink = _permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel),
                AllowPrivateMessages = _forumSettings.AllowPrivateMessages,
                UnreadPrivateMessages = unreadMessage,
                AlertMessage = alertMessage,
            };

            return PartialView(model);
        }

        //footer
        [ChildActionOnly]
        public virtual ActionResult Footer()
        {
            var model = new FooterModel()
            {
                SiteName = _siteInformationSettings.SiteName
            };

            return PartialView(model);
        }

        //menu
        [ChildActionOnly]
        public virtual ActionResult Menu()
        {
            var model = new MenuModel()
            {
                ForumEnabled = _forumSettings.ForumsEnabled
            };

            return PartialView(model);
        }

        //info block
        [ChildActionOnly]
        public virtual ActionResult InfoBlock()
        {
            var model = new InfoBlockModel()
            {
                SitemapEnabled = _commonSettings.SitemapEnabled,
                ForumEnabled = _forumSettings.ForumsEnabled,
                AllowPrivateMessages = _forumSettings.AllowPrivateMessages,
            };

            return PartialView(model);
        }

        //contact us page
        [SSGHttpsRequirement(SslRequirement.No)]
        public virtual ActionResult ContactUs()
        {
            var model = new ContactUsModel()
            {
                Email = _workContext.CurrentUser.Email,
                FullName = _workContext.CurrentUser.GetFullName(),
                DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage
            };
            return View(model);
        }

        [HttpPost, ActionName("ContactUs")]
        [CaptchaValidator]
        public virtual ActionResult ContactUsSend(ContactUsModel model, bool captchaValid)
        {
            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage && !captchaValid)
            {
                ModelState.AddModelError("", _localizationService.GetResource("Common.WrongCaptcha"));
            }

            if (ModelState.IsValid)
            {
                string email = model.Email.Trim();
                string fullName = model.FullName;
                string subject = string.Format(_localizationService.GetResource("ContactUs.EmailSubject"), _siteInformationSettings.SiteName);

                var emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
                if (emailAccount == null)
                    emailAccount = _emailAccountService.GetAllEmailAccounts().FirstOrDefault();

                string from = null;
                string fromName = null;
                string body = Core.Html.HtmlHelper.FormatText(model.Enquiry, false, true, false, false, false, false);
                //required for some SMTP servers
                if (_commonSettings.UseSystemEmailForContactUsForm)
                {
                    from = emailAccount.Email;
                    fromName = emailAccount.DisplayName;
                    body = string.Format("<strong>From</strong>: {0} - {1}<br /><br />{2}", 
                        Server.HtmlEncode(fullName), 
                        Server.HtmlEncode(email), body);
                }
                else
                {
                    from = email;
                    fromName = fullName;
                }
                _queuedEmailService.InsertQueuedEmail(new QueuedEmail()
                {
                    From = from,
                    FromName = fromName,
                    To = emailAccount.Email,
                    ToName = emailAccount.DisplayName,
                    Priority = 5,
                    Subject = subject,
                    Body = body,
                    CreatedOnUtc = DateTime.UtcNow,
                    EmailAccountId = emailAccount.Id
                });
                
                model.SuccessfullySent = true;
                model.Result = _localizationService.GetResource("ContactUs.YourEnquiryHasBeenSent");
                return View(model);
            }

            model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage;
            return View(model);
        }

        //sitemap page
        [SSGHttpsRequirement(SslRequirement.No)]
        public virtual ActionResult Sitemap()
        {
            if (!_commonSettings.SitemapEnabled)
                return RedirectToRoute("HomePage");

            var model = new SitemapModel();
            if (_commonSettings.SitemapIncludeTopics)
            {
                var topics = _topicService.GetAllTopics().ToList().FindAll(t => t.IncludeInSitemap);
                model.Topics = topics.Select(x => x.ToModel()).ToList();
            }
            return View(model);
        }

        //SEO sitemap page
        [SSGHttpsRequirement(SslRequirement.No)]
        public virtual ActionResult SitemapSeo()
        {
            if (!_commonSettings.SitemapEnabled)
                return RedirectToRoute("HomePage");

            string siteMap = _sitemapGenerator.Generate();
            return Content(siteMap, "text/xml");
        }

        //site theme
        [ChildActionOnly]
        public virtual ActionResult SiteThemeSelector()
        {
            if (!_siteInformationSettings.AllowUserToSelectTheme)
                return Content("");

            var model = new SiteThemeSelectorModel();
            var currentTheme = _themeProvider.GetThemeConfiguration(_themeContext.WorkingDesktopTheme);
            model.CurrentSiteTheme = new SiteThemeModel()
            {
                Name = currentTheme.ThemeName,
                Title = currentTheme.ThemeTitle
            };
            model.AvailableSiteThemes = _themeProvider.GetThemeConfigurations()
                //do not display themes for mobile devices
                .Where(x => !x.MobileTheme)
                .Select(x =>
                {
                    return new SiteThemeModel()
                    {
                        Name = x.ThemeName,
                        Title = x.ThemeTitle
                    };
                })
                .ToList();
            return PartialView(model);
        }

        public virtual ActionResult SiteThemeSelected(string themeName)
        {
            _themeContext.WorkingDesktopTheme = themeName;
            
            var model = new SiteThemeSelectorModel();
            var currentTheme = _themeProvider.GetThemeConfiguration(_themeContext.WorkingDesktopTheme);
            model.CurrentSiteTheme = new SiteThemeModel()
            {
                Name = currentTheme.ThemeName,
                Title = currentTheme.ThemeTitle
            };
            model.AvailableSiteThemes = _themeProvider.GetThemeConfigurations()
                //do not display themes for mobile devices
                .Where(x => !x.MobileTheme)
                .Select(x =>
                {
                    return new SiteThemeModel()
                    {
                        Name = x.ThemeName,
                        Title = x.ThemeTitle
                    };
                })
                .ToList();
            return PartialView("SiteThemeSelector", model);
        }

        //favicon
        [ChildActionOnly]
        public virtual ActionResult Favicon()
        {
            var model = new FaviconModel()
            {
                Uploaded = System.IO.File.Exists(System.IO.Path.Combine(Request.PhysicalApplicationPath, "favicon.ico")),
                FaviconUrl = _webHelper.GetSiteLocation() + "favicon.ico"
            };
            
            return PartialView(model);
        }

        /// <summary>
        /// Change presentation layer (desktop or mobile version)
        /// </summary>
        /// <param name="dontUseMobileVersion">True - use desktop version; false - use version for mobile devices</param>
        /// <returns>Action result</returns>
        public virtual ActionResult ChangeDevice(bool dontUseMobileVersion)
        {
            _genericAttributeService.SaveAttribute(_workContext.CurrentUser,
                SystemUserAttributeNames.DontUseMobileVersion, dontUseMobileVersion);

            //TODO: URL referrer is null in IE 8. Fix it
            if (HttpContext.Request.UrlReferrer != null)
            {
                return Redirect(HttpContext.Request.UrlReferrer.PathAndQuery);
            }
            else
            {
                return RedirectToRoute("HomePage");
            }
        }
        [ChildActionOnly]
        public virtual ActionResult ChangeDeviceBlock()
        {
            if (!_mobileDeviceHelper.MobileDevicesSupported())
                //mobile devices support is disabled
                return Content("");

            if (!_mobileDeviceHelper.IsMobileDevice(_httpContext))
                //request is made by a desktop computer
                return Content("");

            return View();
        }





        [ChildActionOnly]
        public virtual ActionResult EuCookieLaw()
        {
            if (!_siteInformationSettings.DisplayEuCookieLawWarning)
                //disabled
                return Content("");

            if (_workContext.CurrentUser.GetAttribute<bool>("EuCookieLaw.Accepted"))
                //already accepted
                return Content("");

            return PartialView();
        }

        [HttpPost]
        public virtual ActionResult EuCookieLawAccept()
        {
            if (!_siteInformationSettings.DisplayEuCookieLawWarning)
                //disabled
                return Json(new { stored = false });

            //save setting
            _genericAttributeService.SaveAttribute(_workContext.CurrentUser, "EuCookieLaw.Accepted", true);
            return Json(new { stored = true });
        }

        #endregion
    }
}
