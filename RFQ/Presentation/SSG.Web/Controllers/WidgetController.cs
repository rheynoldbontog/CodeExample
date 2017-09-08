using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using SSG.Services.Cms;
using SSG.Web.Models.Cms;

namespace SSG.Web.Controllers
{
    public partial class WidgetController : BaseSSGController
    {
		#region Fields

        private readonly IWidgetService _widgetService;

        #endregion

		#region Constructors

        public WidgetController(IWidgetService widgetService)
        {
            this._widgetService = widgetService;
        }

        #endregion

        #region Methods

        [ChildActionOnly]
        public virtual ActionResult WidgetsByZone(string widgetZone)
        {
            //model
            var model = new List<RenderWidgetModel>();

            var widgets = _widgetService.LoadActiveWidgetsByWidgetZone(widgetZone);
            foreach (var widget in widgets)
            {
                var widgetModel = new RenderWidgetModel();

                string actionName;
                string controllerName;
                RouteValueDictionary routeValues;
                widget.GetDisplayWidgetRoute(widgetZone, out actionName, out controllerName, out routeValues);
                widgetModel.ActionName = actionName;
                widgetModel.ControllerName = controllerName;
                widgetModel.RouteValues = routeValues;

                model.Add(widgetModel);
            }

            return PartialView(model);
        }

        #endregion
    }
}
