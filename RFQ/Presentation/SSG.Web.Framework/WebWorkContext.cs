using System;
using System.Linq;
using System.Web;
using SSG.Core;
using SSG.Core.Domain.Directory;
using SSG.Core.Domain.Localization;
using SSG.Core.Domain.Users;
using SSG.Services.Authentication;
using SSG.Services.Common;
using SSG.Services.Directory;
using SSG.Services.Localization;
using SSG.Services.Users;
using SSG.Web.Framework.Localization;

namespace SSG.Web.Framework
{
    /// <summary>
    /// Working context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        private const string UserCookieName = "SSG.user";

        private readonly HttpContextBase _httpContext;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILanguageService _languageService;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly LocalizationSettings _localizationSettings;
        private readonly IWebHelper _webHelper;

        private User _cachedUser;
        private User _originalUserIfImpersonated;
        private bool _cachedIsAdmin;

        public WebWorkContext(HttpContextBase httpContext,
            IUserService userService,
            IAuthenticationService authenticationService,
            ILanguageService languageService,
            ICurrencyService currencyService,
            CurrencySettings currencySettings,
            LocalizationSettings localizationSettings,
            IWebHelper webHelper)
        {
            this._httpContext = httpContext;
            this._userService = userService;
            this._authenticationService = authenticationService;
            this._languageService = languageService;
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._localizationSettings = localizationSettings;
            this._webHelper = webHelper;
        }

        protected User GetCurrentUser()
        {
            if (_cachedUser != null)
                return _cachedUser;

            User user = null;
            if (_httpContext != null)
            {
                //check whether request is made by a search engine
                //in this case return built-in user record for search engines 
                //or comment the following two lines of code in order to disable this functionality
                if (_webHelper.IsSearchEngine(_httpContext))
                    user = _userService.GetUserBySystemName(SystemUserNames.SearchEngine);

                //registered user
                if (user == null || user.Deleted || !user.Active)
                {
                    user = _authenticationService.GetAuthenticatedUser();
                }

                //impersonate user if required (currently used for 'phone order' support)
                if (user != null && !user.Deleted && user.Active)
                {
                        int? impersonatedUserId = user.GetAttribute<int?>(SystemUserAttributeNames.ImpersonatedUserId);
                        if (impersonatedUserId.HasValue && impersonatedUserId.Value > 0)
                        {
                            var impersonatedUser = _userService.GetUserById(impersonatedUserId.Value);
                            if (impersonatedUser != null && !impersonatedUser.Deleted && impersonatedUser.Active)
                            {
                                //set impersonated user
                                _originalUserIfImpersonated = user;
                                user = impersonatedUser;
                            }
                        }
                }

                //load guest user
                if (user == null || user.Deleted || !user.Active)
                {
                    var userCookie = GetUserCookie();
                    if (userCookie != null && !String.IsNullOrEmpty(userCookie.Value))
                    {
                        Guid userGuid;
                        if (Guid.TryParse(userCookie.Value, out userGuid))
                        {
                            var userByCookie = _userService.GetUserByGuid(userGuid);
                            if (userByCookie != null &&
                                //this user (from cookie) should not be registered
                                !userByCookie.IsRegistered() &&
                                //it should not be a built-in 'search engine' user account
                                !userByCookie.IsSearchEngineAccount())
                                user = userByCookie;
                        }
                    }
                }

                //create guest if not exists
                if (user == null || user.Deleted || !user.Active)
                {
                    user = _userService.InsertGuestUser();
                }

                SetUserCookie(user.UserGuid);
            }

            //validation
            if (user != null && !user.Deleted && user.Active)
            {
                //update last activity date
                if (user.LastActivityDateUtc.AddMinutes(1.0) < DateTime.UtcNow)
                {
                    user.LastActivityDateUtc = DateTime.UtcNow;
                    _userService.UpdateUser(user);
                }

                //update IP address
                string currentIpAddress = _webHelper.GetCurrentIpAddress();
                if (!String.IsNullOrEmpty(currentIpAddress))
                {
                    if (!currentIpAddress.Equals(user.LastIpAddress))
                    {
                        user.LastIpAddress = currentIpAddress;
                        _userService.UpdateUser(user);
                    }
                }

                _cachedUser = user;
            }

            return _cachedUser;
        }

        protected HttpCookie GetUserCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            return _httpContext.Request.Cookies[UserCookieName];
        }

        protected void SetUserCookie(Guid userGuid)
        {
            var cookie = new HttpCookie(UserCookieName);
            cookie.Value = userGuid.ToString();
            if (userGuid == Guid.Empty)
            {
                cookie.Expires = DateTime.Now.AddMonths(-1);
            }
            else
            {
                int cookieExpires = 24 * 365; //TODO make configurable
                cookie.Expires = DateTime.Now.AddHours(cookieExpires);
            }
            if (_httpContext != null && _httpContext.Response != null)
            {
                _httpContext.Response.Cookies.Remove(UserCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        public User CurrentUser
        {
            get
            {
                return GetCurrentUser();
            }
            set
            {
                SetUserCookie(value.UserGuid);
                _cachedUser = value;
            }
        }

        /// <summary>
        /// Gets or sets the original user (in case the current one is impersonated)
        /// </summary>
        public User OriginalUserIfImpersonated
        {
            get
            {
                return _originalUserIfImpersonated;
            }
        }

        /// <summary>
        /// Get or set current user working language
        /// </summary>
        public Language WorkingLanguage
        {
            get
            {
                //get language from URL (if possible)
                if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                {
                    if (_httpContext != null)
                    {
                        string virtualPath = _httpContext.Request.AppRelativeCurrentExecutionFilePath;
                        string applicationPath = _httpContext.Request.ApplicationPath;
                        if (virtualPath.IsLocalizedUrl(applicationPath, false))
                        {
                            var seoCode = virtualPath.GetLanguageSeoCodeFromUrl(applicationPath, false);
                            if (!String.IsNullOrEmpty(seoCode))
                            {
                                var langByCulture = _languageService.GetAllLanguages()
                                    .Where(l => seoCode.Equals(l.UniqueSeoCode, StringComparison.InvariantCultureIgnoreCase))
                                    .FirstOrDefault();
                                if (langByCulture != null && langByCulture.Published)
                                {
                                    //the language is found. now we need to save it
                                    if (this.CurrentUser != null &&
                                        !langByCulture.Equals(this.CurrentUser.Language))
                                    {
                                        this.CurrentUser.Language = langByCulture;
                                        _userService.UpdateUser(this.CurrentUser);
                                    }
                                }
                            }
                        }
                    }
                }
                if (this.CurrentUser != null &&
                    this.CurrentUser.Language != null &&
                    this.CurrentUser.Language.Published)
                    return this.CurrentUser.Language;

                var lang = _languageService.GetAllLanguages().FirstOrDefault();
                return lang;
            }
            set
            {
                if (this.CurrentUser == null)
                    return;

                this.CurrentUser.Language = value;
                _userService.UpdateUser(this.CurrentUser);
            }
        }

        /// <summary>
        /// Get or set current user working currency
        /// </summary>
        public Currency WorkingCurrency
        {
            get
            {
                //return primary site currency when we're in admin area/mode
                if (this.IsAdmin)
                {
                    var primarySiteCurrency =  _currencyService.GetCurrencyById(_currencySettings.PrimarySiteCurrencyId);
                    if (primarySiteCurrency != null)
                        return primarySiteCurrency;
                }

                if (this.CurrentUser != null &&
                    this.CurrentUser.Currency != null &&
                    this.CurrentUser.Currency.Published)
                    return this.CurrentUser.Currency;

                var currency = _currencyService.GetAllCurrencies().FirstOrDefault();
                return currency;
            }
            set
            {
                if (this.CurrentUser == null)
                    return;

                this.CurrentUser.Currency = value;
                _userService.UpdateUser(this.CurrentUser);
            }
        }

        public bool IsAdmin
        {
            get
            {
                return _cachedUser.IsInUserRole("Administrators");
            }
            set
            {
                _cachedIsAdmin = value;
            }
        }
    }
}
