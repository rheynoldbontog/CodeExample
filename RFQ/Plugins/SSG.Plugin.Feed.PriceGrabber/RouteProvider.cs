using System.Web.Mvc;
using System.Web.Routing;
using SSG.Web.Framework.Mvc.Routes;

namespace SSG.Plugin.Feed.PriceGrabber
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Feed.PriceGrabber.Configure",
                 "Plugins/FeedPriceGrabber/Configure",
                 new { controller = "FeedPriceGrabber", action = "Configure" },
                 new[] { "SSG.Plugin.Feed.PriceGrabber.Controllers" }
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
