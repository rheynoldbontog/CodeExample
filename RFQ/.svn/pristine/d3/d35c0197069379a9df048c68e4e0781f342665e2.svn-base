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

namespace SSG.Plugin.Feed.PriceGrabber
{
    public class PriceGrabberService : BasePlugin,  IMiscPlugin
    {
        #region Fields

        private readonly IPictureService _pictureService;
        private readonly ICurrencyService _currencyService;
        private readonly IWebHelper _webHelper;
        private readonly ISettingService _settingService;
        private readonly PriceGrabberSettings _priceGrabberSettings;
        private readonly CurrencySettings _currencySettings;

        #endregion

        #region Ctor
        public PriceGrabberService(
            IPictureService pictureService,
            ICurrencyService currencyService, IWebHelper webHelper,
            ISettingService settingService,
            PriceGrabberSettings priceGrabberSettings, CurrencySettings currencySettings)
        {
            this._pictureService = pictureService;
            this._currencyService = currencyService;
            this._webHelper = webHelper;
            this._settingService = settingService;
            this._priceGrabberSettings = priceGrabberSettings;
            this._currencySettings = currencySettings;
        }

        #endregion

        #region Utilities

        private SSG.Core.Domain.Directory.Currency GetUsedCurrency()
        {
            var currency = _currencyService.GetCurrencyById(_priceGrabberSettings.CurrencyId);
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
            controllerName = "FeedPriceGrabber";
            routeValues = new RouteValueDictionary() { { "Namespaces", "SSG.Plugin.Feed.PriceGrabber.Controllers" }, { "area", null } };
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
            var settings = new PriceGrabberSettings()
            {
                ProductPictureSize = 125,
            };
            _settingService.SaveSetting(settings);

            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.PriceGrabber.ClickHere", "Click here");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.PriceGrabber.Currency", "Currency");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.PriceGrabber.Currency.Hint", "Select the default currency that will be used to generate the feed.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.PriceGrabber.Generate", "Generate feed");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.PriceGrabber.ProductPictureSize", "Product thumbnail image size");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.PriceGrabber.ProductPictureSize.Hint", "The default size (pixels) for product thumbnail images.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.PriceGrabber.SuccessResult", "PriceGrabber feed has been successfully generated. {0} to see generated feed");
            
            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<PriceGrabberSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Feed.PriceGrabber.ClickHere");
            this.DeletePluginLocaleResource("Plugins.Feed.PriceGrabber.Currency");
            this.DeletePluginLocaleResource("Plugins.Feed.PriceGrabber.Currency.Hint");
            this.DeletePluginLocaleResource("Plugins.Feed.PriceGrabber.Generate");
            this.DeletePluginLocaleResource("Plugins.Feed.PriceGrabber.ProductPictureSize");
            this.DeletePluginLocaleResource("Plugins.Feed.PriceGrabber.ProductPictureSize.Hint");
            this.DeletePluginLocaleResource("Plugins.Feed.PriceGrabber.SuccessResult");
            
            base.Uninstall();
        }

        #endregion
    }
}
