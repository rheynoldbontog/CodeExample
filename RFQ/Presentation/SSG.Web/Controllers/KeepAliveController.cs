using System.Web.Mvc;

namespace SSG.Web.Controllers
{
    public partial class KeepAliveController : Controller
    {
        public virtual ActionResult Index()
        {
            return Content("I am alive!");
        }
    }
}
