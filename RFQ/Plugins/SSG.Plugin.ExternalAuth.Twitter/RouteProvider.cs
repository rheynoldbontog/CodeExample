using System.Web.Mvc;
using System.Web.Routing;
using SSG.Web.Framework.Mvc.Routes;

namespace SSG.Plugin.ExternalAuth.Twitter
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.ExternalAuth.Twitter.Configure",
                 "Plugins/ExternalAuthTwitter/Configure",
                 new { controller = "ExternalAuthTwitter", action = "Configure" },
                 new[] { "SSG.Plugin.ExternalAuth.Twitter.Controllers" }
            );

            routes.MapRoute("Plugin.ExternalAuth.Twitter.PublicInfo",
                 "Plugins/ExternalAuthTwitter/PublicInfo",
                 new { controller = "ExternalAuthTwitter", action = "PublicInfo" },
                 new[] { "SSG.Plugin.ExternalAuth.Twitter.Controllers" }
            );

            routes.MapRoute("Plugin.ExternalAuth.Twitter.Login",
                 "Plugins/ExternalAuthTwitter/Login",
                 new { controller = "ExternalAuthTwitter", action = "Login" },
                 new[] { "SSG.Plugin.ExternalAuth.Twitter.Controllers" }
            );
        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
