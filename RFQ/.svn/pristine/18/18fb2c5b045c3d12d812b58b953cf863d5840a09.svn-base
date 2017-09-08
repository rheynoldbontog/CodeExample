using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Routing;
using SSG.Core;
using SSG.Core.Domain.Directory;
using SSG.Core.Html;
using SSG.Core.Plugins;
using SSG.Services.Common;
using SSG.Services.Configuration;
using SSG.Services.Directory;
using SSG.Services.Localization;
using SSG.Services.Media;

namespace SSG.Plugin.Feed.Become
{
    public class BecomeService : BasePlugin,  IMiscPlugin
    {
        #region Fields

        private readonly IPictureService _pictureService;
        private readonly ICurrencyService _currencyService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly BecomeSettings _becomeSettings;
        private readonly CurrencySettings _currencySettings;

        #endregion

        #region Ctor
        public BecomeService(IPictureService pictureService,
            ICurrencyService currencyService, IWebHelper webHelper,
            ISettingService settingService,
            BecomeSettings becomeSettings, CurrencySettings currencySettings)
        {
            this._pictureService = pictureService;
            this._currencyService = currencyService;
            this._webHelper = webHelper;
            this._settingService = settingService;
            this._becomeSettings = becomeSettings;
            this._currencySettings = currencySettings;
        }

        #endregion

        #region Utilities

        private SSG.Core.Domain.Directory.Currency GetUsedCurrency()
        {
            var currency = _currencyService.GetCurrencyById(_becomeSettings.CurrencyId);
            if (currency == null || !currency.Published)
                currency = _currencyService.GetCurrencyById(_currencySettings.PrimarySiteCurrencyId);
            return currency;
        }

        private static string RemoveSpecChars(string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;

            s = s.Replace(';', ',');
            s = s.Replace('\r', ' ');
            s = s.Replace('\n', ' ');
            return s;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "FeedBecome";
            routeValues = new RouteValueDictionary() { { "Namespaces", "SSG.Plugin.Feed.Become.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Generate a feed
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>Generated feed</returns>
        public void GenerateFeed(Stream stream)
        {
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //settings
            var settings = new BecomeSettings()
            {
                ProductPictureSize = 125
            };
            _settingService.SaveSetting(settings);

            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Become.ClickHere", "Click here");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Become.Currency", "Currency");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Become.Currency.Hint", "Select the default currency that will be used to generate the feed.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Become.Generate", "Generate feed");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Become.ProductPictureSize", "Product thumbnail image size");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Become.ProductPictureSize.Hint", "The default size (pixels) for product thumbnail images.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Become.SuccessResult", "Become.com feed has been successfully generated. {0} to see generated feed");

            base.Install();
        }
        
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<BecomeSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Feed.Become.ClickHere");
            this.DeletePluginLocaleResource("Plugins.Feed.Become.Currency");
            this.DeletePluginLocaleResource("Plugins.Feed.Become.Currency.Hint");
            this.DeletePluginLocaleResource("Plugins.Feed.Become.Generate");
            this.DeletePluginLocaleResource("Plugins.Feed.Become.ProductPictureSize");
            this.DeletePluginLocaleResource("Plugins.Feed.Become.ProductPictureSize.Hint");
            this.DeletePluginLocaleResource("Plugins.Feed.Become.SuccessResult");

            base.Uninstall();
        }

        #endregion
    }
}
