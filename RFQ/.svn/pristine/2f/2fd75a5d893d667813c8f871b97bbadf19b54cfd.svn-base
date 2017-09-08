using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Routing;
using System.Xml;
using SSG.Core;
using SSG.Core.Domain;
using SSG.Core.Domain.Directory;
using SSG.Core.Domain.Tasks;
using SSG.Core.Plugins;
using SSG.Plugin.Feed.Froogle.Data;
using SSG.Plugin.Feed.Froogle.Services;
using SSG.Services.Common;
using SSG.Services.Configuration;
using SSG.Services.Directory;
using SSG.Services.Localization;
using SSG.Services.Media;
using SSG.Services.Tasks;

namespace SSG.Plugin.Feed.Froogle
{
    public class FroogleService : BasePlugin, IMiscPlugin
    {
        #region Fields

        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly IGoogleService _googleService;
        private readonly IPictureService _pictureService;
        private readonly ICurrencyService _currencyService;
        private readonly IWebHelper _webHelper;
        private readonly ISettingService _settingService;
        private readonly IWorkContext _workContext;
        private readonly IMeasureService _measureService;
        private readonly MeasureSettings _measureSettings;
        private readonly SiteInformationSettings _siteInformationSettings;
        private readonly FroogleSettings _froogleSettings;
        private readonly CurrencySettings _currencySettings;
        private readonly GoogleProductObjectContext _objectContext;

        #endregion

        #region Ctor
        public FroogleService(IScheduleTaskService scheduleTaskService,
            IGoogleService googleService,
            IPictureService pictureService,
            ICurrencyService currencyService,
            IWebHelper webHelper,
            ISettingService settingService,
            IWorkContext workContext,
            IMeasureService measureService,
            MeasureSettings measureSettings,
            SiteInformationSettings siteInformationSettings,
            FroogleSettings froogleSettings,
            CurrencySettings currencySettings,
            GoogleProductObjectContext objectContext)
        {
            this._scheduleTaskService = scheduleTaskService;
            this._googleService = googleService;
            this._pictureService = pictureService;
            this._currencyService = currencyService;
            this._webHelper = webHelper;
            this._settingService = settingService;
            this._workContext = workContext;
            this._measureService = measureService;
            this._measureSettings = measureSettings;
            this._siteInformationSettings = siteInformationSettings;
            this._froogleSettings = froogleSettings;
            this._currencySettings = currencySettings;
            this._objectContext = objectContext;
        }

        #endregion

        #region Utilities

        private Currency GetUsedCurrency()
        {
            var currency = _currencyService.GetCurrencyById(_froogleSettings.CurrencyId);
            if (currency == null || !currency.Published)
                currency = _currencyService.GetCurrencyById(_currencySettings.PrimarySiteCurrencyId);
            return currency;
        }

        private ScheduleTask FindScheduledTask()
        {
            return _scheduleTaskService.GetTaskByType("SSG.Plugin.Feed.Froogle.StaticFileGenerationTask, SSG.Plugin.Feed.Froogle");
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
            controllerName = "FeedFroogle";
            routeValues = new RouteValueDictionary() { { "Namespaces", "SSG.Plugin.Feed.Froogle.Controllers" }, { "area", null } };
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
            var settings = new FroogleSettings()
            {
                ProductPictureSize = 125,
                PassShippingInfo = false,
                FtpHostname = "ftp://uploads.google.com",
                StaticFileName = string.Format("froogle_{0}.xml", CommonHelper.GenerateRandomDigitCode(10)),
            };
            _settingService.SaveSetting(settings);
            
            //data
            _objectContext.Install();

            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.ClickHere", "Click here");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.Currency", "Currency");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.Currency.Hint", "Select the default currency that will be used to generate the feed.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.DefaultGoogleCategory", "Default Google category");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.DefaultGoogleCategory.Hint", "The default Google category will be useds if other one is not specified.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.FtpHostname", "FTP Hostname");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.FtpHostname.Hint", "Google FTP server hostname.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.FtpFilename", "FTP File name");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.FtpFilename.Hint", "Feed file name.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.FtpUsername", "FTP Username");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.FtpUsername.Hint", "Google FTP account username.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.FtpPassword", "FTP Password");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.FtpPassword.Hint", "Google FTP account password.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.FtpUploadStatus", "Froogle feed upload status: {0}");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.General", "General");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.Generate", "Generate feed");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.Override", "Override product settings");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.ProductPictureSize", "Product thumbnail image size");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.ProductPictureSize.Hint", "The default size (pixels) for product thumbnail images.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.Products.ProductName", "Product");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.Products.GoogleCategory", "Google Category");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.SuccessResult", "Froogle feed has been successfully generated. {0} to see generated feed");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.Upload", "Upload feed to Google FTP server");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.TaskEnabled", "Automatically generate a file");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.TaskEnabled.Hint", "Check if you want a file to be automatically generated.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.GenerateStaticFileEachMinutes", "A task period (minutes)");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.GenerateStaticFileEachMinutes.Hint", "Specify a task period in minutes (generation of a new Froogle file).");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.TaskRestart", "If a task settings ('Automatically generate a file') have been changed, please restart the application");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.StaticFilePath", "Generated file path (static)");
            this.AddOrUpdatePluginLocaleResource("Plugins.Feed.Froogle.StaticFilePath.Hint", "A file path of the generated Froogle file. It's static for your site and can be shared with the Froogle service.");

            //install a schedule task
            var task = FindScheduledTask();
            if (task == null)
            {
                task = new ScheduleTask
                {
                    Name = "Froogle static file generation",
                    //each 60 minutes
                    Seconds = 3600,
                    Type = "SSG.Plugin.Feed.Froogle.StaticFileGenerationTask, SSG.Plugin.Feed.Froogle",
                    Enabled = false,
                    StopOnError = false,
                };
                _scheduleTaskService.InsertTask(task);
            }

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<FroogleSettings>();

            //data
            _objectContext.Uninstall();

            //locales
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.ClickHere");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.Currency");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.Currency.Hint");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.DefaultGoogleCategory");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.DefaultGoogleCategory.Hint");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.FtpHostname");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.FtpHostname.Hint");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.FtpFilename");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.FtpFilename.Hint");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.FtpUsername");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.FtpUsername.Hint");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.FtpPassword");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.FtpPassword.Hint");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.FtpUploadStatus");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.General");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.Generate");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.Override");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.ProductPictureSize");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.ProductPictureSize.Hint");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.Products.ProductName");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.Products.GoogleCategory");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.SuccessResult");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.Upload");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.TaskEnabled");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.TaskEnabled.Hint");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.GenerateStaticFileEachMinutes");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.GenerateStaticFileEachMinutes.Hint");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.TaskRestart");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.StaticFilePath");
            this.DeletePluginLocaleResource("Plugins.Feed.Froogle.StaticFilePath.Hint");


            //Remove scheduled task
            var task = FindScheduledTask();
            if (task != null)
                _scheduleTaskService.DeleteTask(task);

            base.Uninstall();
        }
        
        /// <summary>
        /// Generate a static file for froogle
        /// </summary>
        public virtual void GenerateStaticFile()
        {
            string filePath = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "content\\files\\exportimport", _froogleSettings.StaticFileName);
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                GenerateFeed(fs);
            }
        }

        #endregion
    }
}
