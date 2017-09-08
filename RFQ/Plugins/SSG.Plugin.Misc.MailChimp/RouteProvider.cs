using System.Web.Mvc;
using System.Web.Routing;
using SSG.Web.Framework.Mvc.Routes;

namespace SSG.Plugin.Misc.MailChimp
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Misc.MailChimp.Configure", "Plugins/MiscMailChimp/Configure",
                            new { controller = "Settings", action = "Index" },
                            new[] { "SSG.Plugin.Misc.MailChimp.Controllers" }
                );

            routes.MapRoute("Plugin.Misc.MailChimp.WebHook", "Plugins/MiscMailChimp/WebHook/{webHookKey}",
                new { controller = "WebHooks", action = "index" },
                new[] { "SSG.Plugin.Misc.MailChimp.Controllers" });
        }

        public int Priority
        {
            get { return 0; }
        }

    }
}