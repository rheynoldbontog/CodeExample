using System.Web.Routing;
using SSG.Core.Plugins;
using SSG.Services.Authentication.External;
using SSG.Services.Localization;

namespace SSG.Plugin.ExternalAuth.OpenId
{
    /// <summary>
    /// OpenId externalAuth processor
    /// </summary>
    public class OpenIdExternalAuthMethod : BasePlugin, IExternalAuthenticationMethod
    {
        #region Methods
        
        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            //configuration is not required
            actionName = null;
            controllerName = null;
            routeValues = null;
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
            controllerName = "ExternalAuthOpenId";
            routeValues = new RouteValueDictionary() { { "Namespaces", "SSG.Plugin.ExternalAuth.OpenId.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenId.Login", "Login using OpenID account");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenId.YourAccount", "Please click your account provider");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenId.Manually", "Enter manually your OpenID");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenId.SignIn", "Sign In");
            
            base.Install();
        }

        public override void Uninstall()
        {
            //locales
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenId.Login");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenId.YourAccount");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenId.Manually");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenId.SignIn");

            base.Uninstall();
        }

        #endregion
        
    }
}
