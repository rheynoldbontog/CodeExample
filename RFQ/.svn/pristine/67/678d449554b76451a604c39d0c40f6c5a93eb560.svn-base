using System;
using System.Web.Mvc;
using SSG.Core.Plugins;
using SSG.Plugin.Sms.Clickatell.Models;
using SSG.Plugin.SMS.Clickatell;
using SSG.Services.Configuration;
using SSG.Services.Localization;
using SSG.Web.Framework.Controllers;

namespace SSG.Plugin.Sms.Clickatell.Controllers
{
    [AdminAuthorize]
    public class SmsClickatellController : Controller
    {
        private readonly ClickatellSettings _clickatellSettings;
        private readonly ISettingService _settingService;
        private readonly IPluginFinder _pluginFinder;
        private readonly ILocalizationService _localizationService;

        public SmsClickatellController(ClickatellSettings clickatellSettings,
            ISettingService settingService, IPluginFinder pluginFinder,
            ILocalizationService localizationService)
        {
            this._clickatellSettings = clickatellSettings;
            this._settingService = settingService;
            this._pluginFinder = pluginFinder;
            this._localizationService = localizationService;
        }

        public ActionResult Configure()
        {
            var model = new SmsClickatellModel();
            model.Enabled = _clickatellSettings.Enabled;
            model.PhoneNumber = _clickatellSettings.PhoneNumber;
            model.ApiId = _clickatellSettings.ApiId;
            model.Username = _clickatellSettings.Username;
            model.Password = _clickatellSettings.Password;
            return View("SSG.Plugin.SMS.Clickatell.Views.SmsClickatell.Configure", model);
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("save")]
        public ActionResult ConfigurePOST(SmsClickatellModel model)
        {
            if (!ModelState.IsValid)
            {
                return Configure();
            }

            //save settings
            _clickatellSettings.Enabled = model.Enabled;
            _clickatellSettings.PhoneNumber = model.PhoneNumber;
            _clickatellSettings.ApiId = model.ApiId;
            _clickatellSettings.Username = model.Username;
            _clickatellSettings.Password = model.Password;
            _settingService.SaveSetting(_clickatellSettings);

            return View("SSG.Plugin.SMS.Clickatell.Views.SmsClickatell.Configure", model);
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("test-sms")]
        public ActionResult TestSms(SmsClickatellModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.TestMessage))
                {
                    model.TestSmsResult = "Enter test message";
                }
                else
                {
                    var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("Mobile.SMS.Clickatell");
                    if (pluginDescriptor == null)
                        throw new Exception("Cannot load the plugin");
                    var plugin = pluginDescriptor.Instance() as ClickatellSmsProvider;
                    if (plugin == null)
                        throw new Exception("Cannot load the plugin");

                    if (!plugin.SendSms(model.TestMessage))
                    {
                        model.TestSmsResult = _localizationService.GetResource("Plugins.Sms.Clickatell.TestFailed");
                    }
                    else
                    {
                        model.TestSmsResult = _localizationService.GetResource("Plugins.Sms.Clickatell.TestSuccess");
                    }
                }
            }
            catch(Exception exc)
            {
                model.TestSmsResult = exc.ToString();
            }

            return View("SSG.Plugin.SMS.Clickatell.Views.SmsClickatell.Configure", model);
        }
    }
}