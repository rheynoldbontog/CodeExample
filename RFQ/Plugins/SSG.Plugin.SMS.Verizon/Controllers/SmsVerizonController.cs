using System;
using System.Web.Mvc;
using SSG.Core.Plugins;
using SSG.Plugin.Sms.Verizon.Models;
using SSG.Plugin.SMS.Verizon;
using SSG.Services.Configuration;
using SSG.Services.Localization;
using SSG.Web.Framework.Controllers;

namespace SSG.Plugin.Sms.Verizon.Controllers
{
    [AdminAuthorize]
    public class SmsVerizonController : Controller
    {
        private readonly VerizonSettings _verizonSettings;
        private readonly ISettingService _settingService;
        private readonly IPluginFinder _pluginFinder;
        private readonly ILocalizationService _localizationService;

        public SmsVerizonController(VerizonSettings verizonSettings,
            ISettingService settingService, IPluginFinder pluginFinder,
            ILocalizationService localizationService)
        {
            this._verizonSettings = verizonSettings;
            this._settingService = settingService;
            this._pluginFinder = pluginFinder;
            this._localizationService = localizationService;
        }

        public ActionResult Configure()
        {
            var model = new SmsVerizonModel();
            model.Enabled = _verizonSettings.Enabled;
            model.Email = _verizonSettings.Email;
            return View("SSG.Plugin.SMS.Verizon.Views.SmsVerizon.Configure", model);
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("save")]
        public ActionResult ConfigurePOST(SmsVerizonModel model)
        {
            if (!ModelState.IsValid)
            {
                return Configure();
            }

            //save settings
            _verizonSettings.Enabled = model.Enabled;
            _verizonSettings.Email = model.Email;
            _settingService.SaveSetting(_verizonSettings);

            return View("SSG.Plugin.SMS.Verizon.Views.SmsVerizon.Configure", model);
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("test-sms")]
        public ActionResult TestSms(SmsVerizonModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.TestMessage))
                {
                    model.TestSmsResult = "Enter test message";
                }
                else
                {
                    var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("Mobile.SMS.Verizon");
                    if (pluginDescriptor == null)
                        throw new Exception("Cannot load the plugin");
                    var plugin = pluginDescriptor.Instance() as VerizonSmsProvider;
                    if (plugin == null)
                        throw new Exception("Cannot load the plugin");

                    if (!plugin.SendSms(model.TestMessage))
                    {
                        model.TestSmsResult = _localizationService.GetResource("Plugins.Sms.Verizon.TestFailed");
                    }
                    else
                    {
                        model.TestSmsResult = _localizationService.GetResource("Plugins.Sms.Verizon.TestSuccess");
                    }
                }
            }
            catch(Exception exc)
            {
                model.TestSmsResult = exc.ToString();
            }

            return View("SSG.Plugin.SMS.Verizon.Views.SmsVerizon.Configure", model);
        }
    }
}