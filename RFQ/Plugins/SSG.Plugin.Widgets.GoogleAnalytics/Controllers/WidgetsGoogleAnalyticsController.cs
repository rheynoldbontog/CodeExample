using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using SSG.Core;
using SSG.Core.Domain;
using SSG.Plugin.Widgets.GoogleAnalytics.Models;
using SSG.Services.Configuration;
using SSG.Services.Logging;
using SSG.Web.Framework.Controllers;

namespace SSG.Plugin.Widgets.GoogleAnalytics.Controllers
{
    public class WidgetsGoogleAnalyticsController : Controller
    {
        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        private readonly ILogger _logger;
        private readonly GoogleAnalyticsSettings _googleAnalyticsSettings;
        private readonly SiteInformationSettings _siteInformationSettings;

        public WidgetsGoogleAnalyticsController(IWorkContext workContext, ISettingService settingService,
            ILogger logger, GoogleAnalyticsSettings trackingScriptsSettings, SiteInformationSettings siteInformationSettings)
        {
            this._workContext = workContext;
            this._settingService = settingService;
            this._logger = logger;
            this._googleAnalyticsSettings = trackingScriptsSettings;
            this._siteInformationSettings = siteInformationSettings;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            var model = new ConfigurationModel();
            model.GoogleId = _googleAnalyticsSettings.GoogleId;
            model.TrackingScript = _googleAnalyticsSettings.TrackingScript; 
            model.EcommerceScript = _googleAnalyticsSettings.EcommerceScript;
            model.EcommerceDetailScript = _googleAnalyticsSettings.EcommerceDetailScript;

            model.ZoneId = _googleAnalyticsSettings.WidgetZone;
            model.AvailableZones.Add(new SelectListItem() { Text = "<head> HTML tag", Value = "head_html_tag"});
            model.AvailableZones.Add(new SelectListItem() { Text = "Before <body> end HTML tag", Value = "body_end_html_tag_before" });
            
            return View("SSG.Plugin.Widgets.GoogleAnalytics.Views.WidgetsGoogleAnalytics.Configure", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //save settings
            _googleAnalyticsSettings.GoogleId = model.GoogleId;
            _googleAnalyticsSettings.TrackingScript = model.TrackingScript; 
            _googleAnalyticsSettings.EcommerceScript = model.EcommerceScript;
            _googleAnalyticsSettings.EcommerceDetailScript = model.EcommerceDetailScript;
            _googleAnalyticsSettings.WidgetZone = model.ZoneId;
            _settingService.SaveSetting(_googleAnalyticsSettings);

            return Configure();
        }

      
        //<script type="text/javascript"> 

        //var _gaq = _gaq || []; 
        //_gaq.push(['_setAccount', 'UA-XXXXX-X']); 
        //_gaq.push(['_trackPageview']); 

        //(function() { 
        //var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true; 
        //ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js'; 
        //var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s); 
        //})(); 

        //</script>
        private string GetTrackingScript()
        {
            string analyticsTrackingScript = "";
            analyticsTrackingScript = _googleAnalyticsSettings.TrackingScript + "\n";
            analyticsTrackingScript = analyticsTrackingScript.Replace("{GOOGLEID}", _googleAnalyticsSettings.GoogleId);
            analyticsTrackingScript = analyticsTrackingScript.Replace("{ECOMMERCE}", "");
            return analyticsTrackingScript;
        }
        
        //<script type="text/javascript"> 

        //var _gaq = _gaq || []; 
        //_gaq.push(['_setAccount', 'UA-XXXXX-X']); 
        //_gaq.push(['_trackPageview']); 
        //_gaq.push(['_addTrans', 
        //'1234',           // order ID - required 
        //'Acme Clothing',  // affiliation or site name 
        //'11.99',          // total - required 
        //'1.29',           // tax 
        //'5',              // shipping 
        //'San Jose',       // city 
        //'California',     // state or province 
        //'USA'             // country 
        //]); 

        //// add item might be called for every item in the shopping cart 
        //// where your ecommerce engine loops through each item in the cart and 
        //// prints out _addItem for each 
        //_gaq.push(['_addItem', 
        //'1234',           // order ID - required 
        //'DD44',           // SKU/code - required 
        //'T-Shirt',        // product name 
        //'Green Medium',   // category or variation 
        //'11.99',          // unit price - required 
        //'1'               // quantity - required 
        //]); 
        //_gaq.push(['_trackTrans']); //submits transaction to the Analytics servers 

        //(function() { 
        //var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true; 
        //ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js'; 
        //var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s); 
        //})(); 

        //</script>

        private string FixIllegalJavaScriptChars(string text)
        {
            if (String.IsNullOrEmpty(text))
                return text;

            //replace ' with \' (http://stackoverflow.com/questions/4292761/need-to-url-encode-labels-when-tracking-events-with-google-analytics)
            text = text.Replace("'", "\\'");
            return text;
        }
    }
}