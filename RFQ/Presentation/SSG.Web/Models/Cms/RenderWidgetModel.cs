using System.Web.Routing;
using SSG.Web.Framework.Mvc;

namespace SSG.Web.Models.Cms
{
    public partial class RenderWidgetModel : BaseSSGModel
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
    }
}