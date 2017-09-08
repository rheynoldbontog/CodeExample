using System.Linq;
using SSG.Core;
using SSG.Core.Domain;
using SSG.Core.Domain.Users;
using SSG.Services.Common;

namespace SSG.Web.Framework.Themes
{
    /// <summary>
    /// Theme context
    /// </summary>
    public partial class ThemeContext : IThemeContext
    {
        private readonly IWorkContext _workContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly SiteInformationSettings _siteInformationSettings;
        private readonly IThemeProvider _themeProvider;

        private bool _desktopThemeIsCached;
        private string _cachedDesktopThemeName;

        private bool _mobileThemeIsCached;
        private string _cachedMobileThemeName;

        public ThemeContext(IWorkContext workContext, IGenericAttributeService genericAttributeService,
            SiteInformationSettings siteInformationSettings, IThemeProvider themeProvider)
        {
            this._workContext = workContext;
            this._genericAttributeService = genericAttributeService;
            this._siteInformationSettings = siteInformationSettings;
            this._themeProvider = themeProvider;
        }

        /// <summary>
        /// Get or set current theme for desktops (e.g. darkOrange)
        /// </summary>
        public string WorkingDesktopTheme
        {
            get
            {
                if (_desktopThemeIsCached)
                    return _cachedDesktopThemeName;

                string theme = "";
                if (_siteInformationSettings.AllowUserToSelectTheme)
                {
                    if (_workContext.CurrentUser != null)
                        theme = _workContext.CurrentUser.GetAttribute<string>(SystemUserAttributeNames.WorkingDesktopThemeName);
                }

                //default site theme
                if (string.IsNullOrEmpty(theme))
                    theme = _siteInformationSettings.DefaultSiteThemeForDesktops;

                //ensure that theme exists
                if (!_themeProvider.ThemeConfigurationExists(theme))
                    theme = _themeProvider.GetThemeConfigurations()
                        .Where(x => !x.MobileTheme)
                        .FirstOrDefault()
                        .ThemeName;
                
                //cache theme
                this._cachedDesktopThemeName = theme;
                this._desktopThemeIsCached = true;
                return theme;
            }
            set
            {
                if (!_siteInformationSettings.AllowUserToSelectTheme)
                    return;

                if (_workContext.CurrentUser == null)
                    return;

                _genericAttributeService.SaveAttribute(_workContext.CurrentUser, SystemUserAttributeNames.WorkingDesktopThemeName, value);

                //clear cache
                this._desktopThemeIsCached = false;
            }
        }

        /// <summary>
        /// Get current theme for mobile (e.g. Mobile)
        /// </summary>
        public string WorkingMobileTheme
        {
            get
            {
                if (_mobileThemeIsCached)
                    return _cachedMobileThemeName;

                //default site theme
                string theme = _siteInformationSettings.DefaultSiteThemeForMobileDevices;

                //ensure that theme exists
                if (!_themeProvider.ThemeConfigurationExists(theme))
                    theme = _themeProvider.GetThemeConfigurations()
                        .Where(x => x.MobileTheme)
                        .FirstOrDefault()
                        .ThemeName;

                //cache theme
                this._cachedMobileThemeName = theme;
                this._mobileThemeIsCached = true;
                return theme;
            }
        }
    }
}
