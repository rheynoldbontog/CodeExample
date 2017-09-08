using System;
using System.IO;
using System.Web.Mvc;
using SSG.Core;
using SSG.Core.Plugins;
using SSG.Plugin.Feed.PriceGrabber.Models;
using SSG.Services.Configuration;
using SSG.Services.Directory;
using SSG.Services.Localization;
using SSG.Services.Logging;
using SSG.Web.Framework.Controllers;

namespace SSG.Plugin.Feed.PriceGrabber.Controllers
{
    [AdminAuthorize]
    public class FeedPriceGrabberController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly ILocalizationService _localizationService;
        private readonly IPluginFinder _pluginFinder;
        private readonly ILogger _logger;
        private readonly IWebHelper _webHelper;
        private readonly PriceGrabberSettings _priceGrabberSettings;
        private readonly ISettingService _settingService;

        public FeedPriceGrabberController(ICurrencyService currencyService,
            ILocalizationService localizationService, IPluginFinder pluginFinder, 
            ILogger logger, IWebHelper webHelper,
            PriceGrabberSettings priceGrabberSettings, ISettingService settingService)
        {
            this._currencyService = currencyService;
            this._localizationService = localizationService;
            this._pluginFinder = pluginFinder;
            this._logger = logger;
            this._webHelper = webHelper;
            this._priceGrabberSettings = priceGrabberSettings;
            this._settingService = settingService;
        }

        public ActionResult Configure()
        {
            var model = new FeedPriceGrabberModel();
            model.ProductPictureSize = _priceGrabberSettings.ProductPictureSize;
            model.CurrencyId = _priceGrabberSettings.CurrencyId;
            foreach (var c in _currencyService.GetAllCurrencies(false))
            {
                model.AvailableCurrencies.Add(new SelectListItem()
                    {
                         Text = c.Name,
                         Value = c.Id.ToString()
                    });
            }

            return View("SSG.Plugin.Feed.PriceGrabber.Views.FeedPriceGrabber.Configure", model);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult Configure(FeedPriceGrabberModel model)
        {
            if (!ModelState.IsValid)
            {
                return Configure();
            }
            
            //save settings
            _priceGrabberSettings.ProductPictureSize = model.ProductPictureSize;
            _priceGrabberSettings.CurrencyId = model.CurrencyId;
            _settingService.SaveSetting(_priceGrabberSettings);

            //redisplay the form
            foreach (var c in _currencyService.GetAllCurrencies(false))
            {
                model.AvailableCurrencies.Add(new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            }
            return View("SSG.Plugin.Feed.PriceGrabber.Views.FeedPriceGrabber.Configure", model);
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("generate")]
        public ActionResult GenerateFeed(FeedPriceGrabberModel model)
        {
            if (!ModelState.IsValid)
            {
                return Configure();
            }


            try
            {
                string fileName = string.Format("priceGrabber_{0}_{1}.csv", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), CommonHelper.GenerateRandomDigitCode(4));
                string filePath = System.IO.Path.Combine(Request.PhysicalApplicationPath, "content\\files\\exportimport", fileName);
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("PromotionFeed.PriceGrabber");
                    if (pluginDescriptor == null)
                        throw new Exception("Cannot load the plugin");

                    //plugin
                    var plugin = pluginDescriptor.Instance() as PriceGrabberService;
                    if (plugin == null)
                        throw new Exception("Cannot load the plugin");

                    plugin.GenerateFeed(fs);
                }

                string clickhereStr = string.Format("<a href=\"{0}content/files/exportimport/{1}\" target=\"_blank\">{2}</a>", _webHelper.GetSiteLocation(false), fileName, _localizationService.GetResource("Plugins.Feed.PriceGrabber.ClickHere"));
                string result = string.Format(_localizationService.GetResource("Plugins.Feed.PriceGrabber.SuccessResult"), clickhereStr);
                model.GenerateFeedResult = result;
            }
            catch (Exception exc)
            {
                model.GenerateFeedResult = exc.Message;
                _logger.Error(exc.Message, exc);
            }


            foreach (var c in _currencyService.GetAllCurrencies(false))
            {
                model.AvailableCurrencies.Add(new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            }
            return View("SSG.Plugin.Feed.PriceGrabber.Views.FeedPriceGrabber.Configure", model);
        }
    }
}
