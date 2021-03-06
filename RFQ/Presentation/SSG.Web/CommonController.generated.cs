// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace SSG.Web.Controllers
{
    public partial class CommonController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected CommonController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult SetLanguage()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SetLanguage);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult CurrencySelected()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CurrencySelected);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ContactUsSend()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ContactUsSend);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult SiteThemeSelected()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SiteThemeSelected);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ChangeDevice()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ChangeDevice);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public CommonController Actions { get { return MVC.Common; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Common";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Common";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string LanguageSelector = "LanguageSelector";
            public readonly string SetLanguage = "SetLanguage";
            public readonly string CurrencySelector = "CurrencySelector";
            public readonly string CurrencySelected = "CurrencySelected";
            public readonly string ConfigButton = "ConfigButton";
            public readonly string Config = "Config";
            public readonly string JavaScriptDisabledWarning = "JavaScriptDisabledWarning";
            public readonly string HeaderLinks = "HeaderLinks";
            public readonly string Footer = "Footer";
            public readonly string Menu = "Menu";
            public readonly string InfoBlock = "InfoBlock";
            public readonly string ContactUs = "ContactUs";
            public readonly string ContactUsSend = "ContactUs";
            public readonly string Sitemap = "Sitemap";
            public readonly string SitemapSeo = "SitemapSeo";
            public readonly string SiteThemeSelector = "SiteThemeSelector";
            public readonly string SiteThemeSelected = "SiteThemeSelected";
            public readonly string Favicon = "Favicon";
            public readonly string ChangeDevice = "ChangeDevice";
            public readonly string ChangeDeviceBlock = "ChangeDeviceBlock";
            public readonly string EuCookieLaw = "EuCookieLaw";
            public readonly string EuCookieLawAccept = "EuCookieLawAccept";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string LanguageSelector = "LanguageSelector";
            public const string SetLanguage = "SetLanguage";
            public const string CurrencySelector = "CurrencySelector";
            public const string CurrencySelected = "CurrencySelected";
            public const string ConfigButton = "ConfigButton";
            public const string Config = "Config";
            public const string JavaScriptDisabledWarning = "JavaScriptDisabledWarning";
            public const string HeaderLinks = "HeaderLinks";
            public const string Footer = "Footer";
            public const string Menu = "Menu";
            public const string InfoBlock = "InfoBlock";
            public const string ContactUs = "ContactUs";
            public const string ContactUsSend = "ContactUs";
            public const string Sitemap = "Sitemap";
            public const string SitemapSeo = "SitemapSeo";
            public const string SiteThemeSelector = "SiteThemeSelector";
            public const string SiteThemeSelected = "SiteThemeSelected";
            public const string Favicon = "Favicon";
            public const string ChangeDevice = "ChangeDevice";
            public const string ChangeDeviceBlock = "ChangeDeviceBlock";
            public const string EuCookieLaw = "EuCookieLaw";
            public const string EuCookieLawAccept = "EuCookieLawAccept";
        }


        static readonly ActionParamsClass_SetLanguage s_params_SetLanguage = new ActionParamsClass_SetLanguage();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SetLanguage SetLanguageParams { get { return s_params_SetLanguage; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SetLanguage
        {
            public readonly string langid = "langid";
        }
        static readonly ActionParamsClass_CurrencySelected s_params_CurrencySelected = new ActionParamsClass_CurrencySelected();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CurrencySelected CurrencySelectedParams { get { return s_params_CurrencySelected; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CurrencySelected
        {
            public readonly string userCurrency = "userCurrency";
        }
        static readonly ActionParamsClass_ContactUsSend s_params_ContactUsSend = new ActionParamsClass_ContactUsSend();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ContactUsSend ContactUsSendParams { get { return s_params_ContactUsSend; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ContactUsSend
        {
            public readonly string model = "model";
            public readonly string captchaValid = "captchaValid";
        }
        static readonly ActionParamsClass_SiteThemeSelected s_params_SiteThemeSelected = new ActionParamsClass_SiteThemeSelected();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SiteThemeSelected SiteThemeSelectedParams { get { return s_params_SiteThemeSelected; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SiteThemeSelected
        {
            public readonly string themeName = "themeName";
        }
        static readonly ActionParamsClass_ChangeDevice s_params_ChangeDevice = new ActionParamsClass_ChangeDevice();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ChangeDevice ChangeDeviceParams { get { return s_params_ChangeDevice; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ChangeDevice
        {
            public readonly string dontUseMobileVersion = "dontUseMobileVersion";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string ChangeDeviceBlock = "ChangeDeviceBlock";
                public readonly string ChangeDeviceBlock_Mobile = "ChangeDeviceBlock.Mobile";
                public readonly string Config = "Config";
                public readonly string Config_Mobile = "Config.Mobile";
                public readonly string ConfigButton_Mobile = "ConfigButton.Mobile";
                public readonly string ContactUs = "ContactUs";
                public readonly string ContactUs_Mobile = "ContactUs.Mobile";
                public readonly string CurrencySelector = "CurrencySelector";
                public readonly string CurrencySelector_Mobile = "CurrencySelector.Mobile";
                public readonly string Favicon = "Favicon";
                public readonly string Footer = "Footer";
                public readonly string Footer_Mobile = "Footer.Mobile";
                public readonly string HeaderLinks = "HeaderLinks";
                public readonly string HeaderLinks_Mobile = "HeaderLinks.Mobile";
                public readonly string InfoBlock = "InfoBlock";
                public readonly string InfoBlock_Mobile = "InfoBlock.Mobile";
                public readonly string JavaScriptDisabledWarning = "JavaScriptDisabledWarning";
                public readonly string LanguageSelector = "LanguageSelector";
                public readonly string LanguageSelector_Mobile = "LanguageSelector.Mobile";
                public readonly string Menu = "Menu";
                public readonly string Sitemap = "Sitemap";
                public readonly string Sitemap_Mobile = "Sitemap.Mobile";
                public readonly string SiteThemeSelector = "SiteThemeSelector";
            }
            public readonly string ChangeDeviceBlock = "~/Views/Common/ChangeDeviceBlock.cshtml";
            public readonly string ChangeDeviceBlock_Mobile = "~/Views/Common/ChangeDeviceBlock.Mobile.cshtml";
            public readonly string Config = "~/Views/Common/Config.cshtml";
            public readonly string Config_Mobile = "~/Views/Common/Config.Mobile.cshtml";
            public readonly string ConfigButton_Mobile = "~/Views/Common/ConfigButton.Mobile.cshtml";
            public readonly string ContactUs = "~/Views/Common/ContactUs.cshtml";
            public readonly string ContactUs_Mobile = "~/Views/Common/ContactUs.Mobile.cshtml";
            public readonly string CurrencySelector = "~/Views/Common/CurrencySelector.cshtml";
            public readonly string CurrencySelector_Mobile = "~/Views/Common/CurrencySelector.Mobile.cshtml";
            public readonly string Favicon = "~/Views/Common/Favicon.cshtml";
            public readonly string Footer = "~/Views/Common/Footer.cshtml";
            public readonly string Footer_Mobile = "~/Views/Common/Footer.Mobile.cshtml";
            public readonly string HeaderLinks = "~/Views/Common/HeaderLinks.cshtml";
            public readonly string HeaderLinks_Mobile = "~/Views/Common/HeaderLinks.Mobile.cshtml";
            public readonly string InfoBlock = "~/Views/Common/InfoBlock.cshtml";
            public readonly string InfoBlock_Mobile = "~/Views/Common/InfoBlock.Mobile.cshtml";
            public readonly string JavaScriptDisabledWarning = "~/Views/Common/JavaScriptDisabledWarning.cshtml";
            public readonly string LanguageSelector = "~/Views/Common/LanguageSelector.cshtml";
            public readonly string LanguageSelector_Mobile = "~/Views/Common/LanguageSelector.Mobile.cshtml";
            public readonly string Menu = "~/Views/Common/Menu.cshtml";
            public readonly string Sitemap = "~/Views/Common/Sitemap.cshtml";
            public readonly string Sitemap_Mobile = "~/Views/Common/Sitemap.Mobile.cshtml";
            public readonly string SiteThemeSelector = "~/Views/Common/SiteThemeSelector.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_CommonController : SSG.Web.Controllers.CommonController
    {
        public T4MVC_CommonController() : base(Dummy.Instance) { }

        [NonAction]
        partial void LanguageSelectorOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult LanguageSelector()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LanguageSelector);
            LanguageSelectorOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SetLanguageOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int langid);

        [NonAction]
        public override System.Web.Mvc.ActionResult SetLanguage(int langid)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SetLanguage);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "langid", langid);
            SetLanguageOverride(callInfo, langid);
            return callInfo;
        }

        [NonAction]
        partial void CurrencySelectorOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult CurrencySelector()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CurrencySelector);
            CurrencySelectorOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void CurrencySelectedOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int userCurrency);

        [NonAction]
        public override System.Web.Mvc.ActionResult CurrencySelected(int userCurrency)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CurrencySelected);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "userCurrency", userCurrency);
            CurrencySelectedOverride(callInfo, userCurrency);
            return callInfo;
        }

        [NonAction]
        partial void ConfigButtonOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult ConfigButton()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ConfigButton);
            ConfigButtonOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ConfigOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Config()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Config);
            ConfigOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void JavaScriptDisabledWarningOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult JavaScriptDisabledWarning()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.JavaScriptDisabledWarning);
            JavaScriptDisabledWarningOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void HeaderLinksOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult HeaderLinks()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.HeaderLinks);
            HeaderLinksOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void FooterOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Footer()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Footer);
            FooterOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void MenuOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Menu()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Menu);
            MenuOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void InfoBlockOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult InfoBlock()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.InfoBlock);
            InfoBlockOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ContactUsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult ContactUs()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ContactUs);
            ContactUsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ContactUsSendOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, SSG.Web.Models.Common.ContactUsModel model, bool captchaValid);

        [NonAction]
        public override System.Web.Mvc.ActionResult ContactUsSend(SSG.Web.Models.Common.ContactUsModel model, bool captchaValid)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ContactUsSend);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "captchaValid", captchaValid);
            ContactUsSendOverride(callInfo, model, captchaValid);
            return callInfo;
        }

        [NonAction]
        partial void SitemapOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Sitemap()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Sitemap);
            SitemapOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SitemapSeoOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult SitemapSeo()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SitemapSeo);
            SitemapSeoOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SiteThemeSelectorOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult SiteThemeSelector()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SiteThemeSelector);
            SiteThemeSelectorOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SiteThemeSelectedOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string themeName);

        [NonAction]
        public override System.Web.Mvc.ActionResult SiteThemeSelected(string themeName)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SiteThemeSelected);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "themeName", themeName);
            SiteThemeSelectedOverride(callInfo, themeName);
            return callInfo;
        }

        [NonAction]
        partial void FaviconOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Favicon()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Favicon);
            FaviconOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ChangeDeviceOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, bool dontUseMobileVersion);

        [NonAction]
        public override System.Web.Mvc.ActionResult ChangeDevice(bool dontUseMobileVersion)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ChangeDevice);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "dontUseMobileVersion", dontUseMobileVersion);
            ChangeDeviceOverride(callInfo, dontUseMobileVersion);
            return callInfo;
        }

        [NonAction]
        partial void ChangeDeviceBlockOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult ChangeDeviceBlock()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ChangeDeviceBlock);
            ChangeDeviceBlockOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void EuCookieLawOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult EuCookieLaw()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EuCookieLaw);
            EuCookieLawOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void EuCookieLawAcceptOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult EuCookieLawAccept()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EuCookieLawAccept);
            EuCookieLawAcceptOverride(callInfo);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
