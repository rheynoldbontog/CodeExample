using System;
using System.Web;
using System.Web.Mvc;
using SSG.Core;
using SSG.Core.Data;
using SSG.Core.Domain;
using SSG.Core.Infrastructure;
using SSG.Services.Users;

namespace SSG.Web.Framework
{
    public class SiteClosedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.HttpContext == null)
                return;

            HttpRequestBase request = filterContext.HttpContext.Request;
            if (request == null)
                return;

            string actionName = filterContext.ActionDescriptor.ActionName;
            if (String.IsNullOrEmpty(actionName))
                return;

            string controllerName = filterContext.Controller.ToString();
            if (String.IsNullOrEmpty(controllerName))
                return;

            //don't apply filter to child methods
            if (filterContext.IsChildAction)
                return;

            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            var siteInformationSettings = EngineContext.Current.Resolve<SiteInformationSettings>();
            if (siteInformationSettings.SiteClosed &&
                //ensure it's not the Login page
                !(controllerName.Equals("SSG.Web.Controllers.UserController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("Login", StringComparison.InvariantCultureIgnoreCase)) &&
                //ensure it's not the Logout page
                !(controllerName.Equals("SSG.Web.Controllers.UserController", StringComparison.InvariantCultureIgnoreCase) && actionName.Equals("Logout", StringComparison.InvariantCultureIgnoreCase)))
            {
                if (siteInformationSettings.SiteClosedAllowForAdmins && EngineContext.Current.Resolve<IWorkContext>().CurrentUser.IsAdmin())
                {
                    //do nothing - allow admin access
                }
                else
                {
                    var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    filterContext.Result = new RedirectResult(webHelper.GetSiteLocation() + "SiteClosed.htm");
                }
            }
        }
    }
}
