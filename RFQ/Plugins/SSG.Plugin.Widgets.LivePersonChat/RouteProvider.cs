using System.Web.Mvc;
using System.Web.Routing;
using SSG.Web.Framework.Mvc.Routes;

namespace SSG.Plugin.Widgets.LivePersonChat
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Widgets.LivePersonChat.Configure",
                 "Plugins/WidgetsLivePersonChat/Configure",
                 new { controller = "WidgetsLivePersonChat", action = "Configure" },
                 new[] { "SSG.Plugin.Widgets.LivePersonChat.Controllers" }
            );

            routes.MapRoute("Plugin.Widgets.LivePersonChat.PublicInfo",
                 "Plugins/WidgetsLivePersonChat/PublicInfo",
                 new { controller = "WidgetsLivePersonChat", action = "PublicInfo" },
                 new[] { "SSG.Plugin.Widgets.LivePersonChat.Controllers" }
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
