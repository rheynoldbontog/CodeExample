using System.Web.Mvc;
using SSG.Web.Framework.Controllers;

namespace SSG.Plugin.Misc.WebServices.Controllers
{
    [AdminAuthorize]
    public class MiscWebServicesController : Controller
    {
        public ActionResult Configure()
        {
            return View("SSG.Plugin.Misc.WebServices.Views.MiscWebServices.Configure");
        }
    }
}
