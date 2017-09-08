using System.Web.Mvc;
using System.Web.Routing;
using SSG.Web.Framework.Mvc.Routes;

namespace SSG.Plugin.Feed.Become
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Feed.Become.Configure",
                 "Plugins/FeedBecome/Configure",
                 new { controller = "FeedBecome", action = "Configure" },
                 new[] { "SSG.Plugin.Feed.Become.Controllers" }
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
