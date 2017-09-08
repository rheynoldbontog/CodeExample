using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSG.Core.Infrastructure;
using SSG.Services.Users;

namespace SSG.Web.ActionFilter
{
    public class UserRoleActionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var _workContext = EngineContext.Current.Resolve<SSG.Core.IWorkContext>();

            filterContext.Controller.ViewBag.IsRequestor = _workContext.CurrentUser.IsRequestor();
            filterContext.Controller.ViewBag.IsRegistered = _workContext.CurrentUser.IsRequestor();
            filterContext.Controller.ViewBag.IsBuyer = _workContext.CurrentUser.IsBuyer();
            filterContext.Controller.ViewBag.IsAdmin = _workContext.CurrentUser.IsAdmin();

        }
    }
}