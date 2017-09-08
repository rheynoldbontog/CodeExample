using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SSG.Core;
using SSG.Core.Domain.Tasks;
using SSG.Core.Plugins;
using SSG.Plugin.Feed.Froogle.Domain;
using SSG.Plugin.Feed.Froogle.Models;
using SSG.Plugin.Feed.Froogle.Services;
using SSG.Services.Configuration;
using SSG.Services.Directory;
using SSG.Services.Localization;
using SSG.Services.Logging;
using SSG.Services.Tasks;
using SSG.Web.Framework.Controllers;
using Telerik.Web.Mvc;

namespace SSG.Plugin.Feed.Froogle.Controllers
{
    [AdminAuthorize]
    public class FeedFroogleController : Controller
    {
        private readonly IGoogleService _googleService;
        private readonly ICurrencyService _currencyService;
        private readonly ILocalizationService _localizationService;
        private readonly IPluginFinder _pluginFinder;
        private readonly ILogger _logger;
        private readonly IWebHelper _webHelper;
        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly FroogleSettings _froogleSettings;
        private readonly ISettingService _settingService;

        public FeedFroogleController(IGoogleService googleService, 
            ICurrencyService currencyService,
            ILocalizationService localizationService, IPluginFinder pluginFinder, 
            ILogger logger, IWebHelper webHelper, IScheduleTaskService scheduleTaskService, 
            FroogleSettings froogleSettings, ISettingService settingService)
        {
            this._googleService = googleService;
            this._currencyService = currencyService;
            this._localizationService = localizationService;
            this._pluginFinder = pluginFinder;
            this._logger = logger;
            this._webHelper = webHelper;
            this._scheduleTaskService = scheduleTaskService;
            this._froogleSettings = froogleSettings;
            this._settingService = settingService;
        }

        [NonAction]
        private ScheduleTask FindScheduledTask()
        {
            return _scheduleTaskService.GetTaskByType("SSG.Plugin.Feed.Froogle.StaticFileGenerationTask, SSG.Plugin.Feed.Froogle");
        }
        
        public ActionResult Configure()
        {
            var model = new FeedFroogleModel();
            //Picture
            model.ProductPictureSize = _froogleSettings.ProductPictureSize;
            //Currency
            model.CurrencyId = _froogleSettings.CurrencyId;
            foreach (var c in _currencyService.GetAllCurrencies(false))
            {
                model.AvailableCurrencies.Add(new SelectListItem()
                    {
                         Text = c.Name,
                         Value = c.Id.ToString()
                    });
            }
            //Google category
            model.DefaultGoogleCategory = _froogleSettings.DefaultGoogleCategory;
            model.AvailableGoogleCategories.Add(new SelectListItem()
            {
                Text = "Select a category",
                Value = ""
            });
            foreach (var gc in _googleService.GetTaxonomyList())
            {
                model.AvailableGoogleCategories.Add(new SelectListItem()
                {
                    Text = gc,
                    Value = gc
                });
            }
            //FTP settings
            model.FtpHostname = _froogleSettings.FtpHostname;
            model.FtpFilename = _froogleSettings.FtpFilename;
            model.FtpUsername = _froogleSettings.FtpUsername;
            model.FtpPassword = _froogleSettings.FtpPassword;
            
            //task
            ScheduleTask task = FindScheduledTask();
            if (task != null)
            {
                model.GenerateStaticFileEachMinutes = task.Seconds / 60;
                model.TaskEnabled = task.Enabled;
            }
            //file path
            if (System.IO.File.Exists(System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "content\\files\\exportimport", _froogleSettings.StaticFileName)))
                model.StaticFilePath = string.Format("{0}content/files/exportimport/{1}", _webHelper.GetSiteLocation(false), _froogleSettings.StaticFileName);

            return View("SSG.Plugin.Feed.Froogle.Views.FeedFroogle.Configure", model);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult Configure(FeedFroogleModel model)
        {
            if (!ModelState.IsValid)
            {
                return Configure();
            }

            string saveResult = "";
            //save settings
            _froogleSettings.ProductPictureSize = model.ProductPictureSize;
            _froogleSettings.CurrencyId = model.CurrencyId;
            _froogleSettings.DefaultGoogleCategory = model.DefaultGoogleCategory;
            _froogleSettings.FtpHostname = model.FtpHostname;
            _froogleSettings.FtpFilename = model.FtpFilename;
            _froogleSettings.FtpUsername = model.FtpUsername;
            _froogleSettings.FtpPassword = model.FtpPassword;
            _settingService.SaveSetting(_froogleSettings);

            // Update the task
            var task = FindScheduledTask();
            if (task != null)
            {
                task.Enabled = model.TaskEnabled;
                task.Seconds = model.GenerateStaticFileEachMinutes * 60;
                _scheduleTaskService.UpdateTask(task);
                saveResult = _localizationService.GetResource("Plugins.Feed.Froogle.TaskRestart");
            }

            //redisplay the form
            foreach (var c in _currencyService.GetAllCurrencies(false))
            {
                model.AvailableCurrencies.Add(new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            }
            model.AvailableGoogleCategories.Add(new SelectListItem()
            {
                Text = "Select a category",
                Value = ""
            });
            foreach (var gc in _googleService.GetTaxonomyList())
            {
                model.AvailableGoogleCategories.Add(new SelectListItem()
                {
                    Text = gc,
                    Value = gc
                });
            }
            //file path
            if (System.IO.File.Exists(System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "content\\files\\exportimport", _froogleSettings.StaticFileName)))
                model.StaticFilePath = string.Format("{0}content/files/exportimport/{1}", _webHelper.GetSiteLocation(false), _froogleSettings.StaticFileName);

            //set result text
            model.SaveResult = saveResult;
            return View("SSG.Plugin.Feed.Froogle.Views.FeedFroogle.Configure", model);
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("generate")]
        public ActionResult GenerateFeed(FeedFroogleModel model)
        {
            if (!ModelState.IsValid)
            {
                return Configure();
            }


            try
            {
                var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("PromotionFeed.Froogle");
                if (pluginDescriptor == null)
                    throw new Exception("Cannot load the plugin");

                //plugin
                var plugin = pluginDescriptor.Instance() as FroogleService;
                if (plugin == null)
                    throw new Exception("Cannot load the plugin");

                plugin.GenerateStaticFile();

                string clickhereStr = string.Format("<a href=\"{0}content/files/exportimport/{1}\" target=\"_blank\">{2}</a>", _webHelper.GetSiteLocation(false), _froogleSettings.StaticFileName, _localizationService.GetResource("Plugins.Feed.Froogle.ClickHere"));
                string result = string.Format(_localizationService.GetResource("Plugins.Feed.Froogle.SuccessResult"), clickhereStr);
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
            model.AvailableGoogleCategories.Add(new SelectListItem()
            {
                Text = "Select a category",
                Value = ""
            });
            foreach (var gc in _googleService.GetTaxonomyList())
            {
                model.AvailableGoogleCategories.Add(new SelectListItem()
                {
                    Text = gc,
                    Value = gc
                });
            }

            //task
            ScheduleTask task = FindScheduledTask();
            if (task != null)
            {
                model.GenerateStaticFileEachMinutes = task.Seconds / 60;
                model.TaskEnabled = task.Enabled;
            }

            //file path
            if (System.IO.File.Exists(System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "content\\files\\exportimport", _froogleSettings.StaticFileName)))
                model.StaticFilePath = string.Format("{0}content/files/exportimport/{1}", _webHelper.GetSiteLocation(false), _froogleSettings.StaticFileName);

            return View("SSG.Plugin.Feed.Froogle.Views.FeedFroogle.Configure", model);
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("uploadfeed")]
        public ActionResult UploadFeed(FeedFroogleModel model)
        {
            if (!ModelState.IsValid)
            {
                return Configure();
            }

            try
            {
                string uri = String.Format("{0}/{1}", _froogleSettings.FtpHostname, _froogleSettings.FtpFilename);
                var req = WebRequest.Create(uri) as FtpWebRequest;
                req.Credentials = new NetworkCredential(_froogleSettings.FtpUsername, _froogleSettings.FtpPassword);
                req.KeepAlive = true;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.UploadFile;

                using (Stream reqStream = req.GetRequestStream())
                {
                    var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("PromotionFeed.Froogle");
                    if (pluginDescriptor == null)
                        throw new Exception("Cannot load the plugin");

                    //plugin
                    var plugin = pluginDescriptor.Instance() as FroogleService;
                    if (plugin == null)
                        throw new Exception("Cannot load the plugin");

                    plugin.GenerateFeed(reqStream);
                }

                var rsp = req.GetResponse() as FtpWebResponse;

                model.GenerateFeedResult = String.Format(_localizationService.GetResource("Plugins.Feed.Froogle.FtpUploadStatus"), rsp.StatusDescription);
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
            model.AvailableGoogleCategories.Add(new SelectListItem()
            {
                Text = "Select a category",
                Value = ""
            });
            foreach (var gc in _googleService.GetTaxonomyList())
            {
                model.AvailableGoogleCategories.Add(new SelectListItem()
                {
                    Text = gc,
                    Value = gc
                });
            }

            //task
            ScheduleTask task = FindScheduledTask();
            if (task != null)
            {
                model.GenerateStaticFileEachMinutes = task.Seconds / 60;
                model.TaskEnabled = task.Enabled;
            }

            //file path
            if (System.IO.File.Exists(System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "content\\files\\exportimport", _froogleSettings.StaticFileName)))
                model.StaticFilePath = string.Format("{0}content/files/exportimport/{1}", _webHelper.GetSiteLocation(false), _froogleSettings.StaticFileName);


            return View("SSG.Plugin.Feed.Froogle.Views.FeedFroogle.Configure", model);
        }

    }
}
