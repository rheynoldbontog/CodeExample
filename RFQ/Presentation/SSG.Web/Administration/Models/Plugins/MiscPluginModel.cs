using System.Web.Routing;
using SSG.Web.Framework.Mvc;

namespace SSG.Admin.Models.Plugins
{
    public partial class MiscPluginModel : BaseSSGModel
    {
        public string FriendlyName { get; set; }

        public string ConfigurationActionName { get; set; }
        public string ConfigurationControllerName { get; set; }
        public RouteValueDictionary ConfigurationRouteValues { get; set; }
    }
}