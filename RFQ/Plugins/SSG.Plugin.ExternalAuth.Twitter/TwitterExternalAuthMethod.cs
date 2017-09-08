using System.Web.Routing;
using SSG.Core.Plugins;
using SSG.Services.Authentication.External;
using SSG.Services.Configuration;
using SSG.Services.Localization;

namespace SSG.Plugin.ExternalAuth.Twitter
{
    /// <summary>
    /// Twitter externalAuth processor
    /// </summary>
    public class TwitterExternalAuthMethod : BasePlugin, IExternalAuthenticationMethod
    {
        #region Fields

        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public TwitterExternalAuthMethod(ISettingService settingService)
        {
            this._settingService = settingService;
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
            controllerName = "ExternalAuthTwitter";
            routeValues = new RouteValueDictionary() { { "Namespaces", "SSG.Plugin.ExternalAuth.Twitter.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for displaying plugin in public site
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetPublicInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "ExternalAuthTwitter";
            routeValues = new RouteValueDictionary() { { "Namespaces", "SSG.Plugin.ExternalAuth.Twitter.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //settings
            var settings = new TwitterExternalAuthSettings()
            {
                ConsumerKey = "",
                ConsumerSecret = "",
            };
            _settingService.SaveSetting(settings);

            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.Twitter.Login", "Login using Twitter account");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.Twitter.ConsumerKey", "Consumer key");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.Twitter.ConsumerKey.Hint", "Enter your consumer key here.");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.Twitter.ConsumerSecret", "Consumer secret");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.Twitter.ConsumerSecret.Hint", "Enter your consumer secret here.");

            base.Install();
        }

        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<TwitterExternalAuthSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.Twitter.Login");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.Twitter.ConsumerKey");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.Twitter.ConsumerKey.Hint");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.Twitter.ConsumerSecret");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.Twitter.ConsumerSecret.Hint");
            
            base.Uninstall();
        }

        #endregion
        
    }
}
