using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SSG.Core;
using SSG.Core.Infrastructure;
using SSG.Services.Logging;
using SSG.Web.Framework;
using SSG.Web.Framework.Security;
using SSG.Web.Framework.UI;
using SSG.Services.Localization;
using SSG.Web.Framework.Localization;
using SSG.Services.Users;

namespace SSG.Web.Controllers
{
    [SiteLastVisitedPage]
    [SiteClosedAttribute]
    [PublicSiteAllowNavigation]
    [SSGHttpsRequirement(SslRequirement.NoMatter)]
    public abstract partial class BaseSSGController : Controller
    {
        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="exc">Exception</param>
        private void LogException(Exception exc)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var logger = EngineContext.Current.Resolve<ILogger>();

            var user = workContext.CurrentUser;
            logger.Error(exc.Message, exc, user);
        }
        /// <summary>
        /// Display success notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="logException">A value indicating whether exception should be logged</param>
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
            if (logException)
                LogException(exception);
            AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display notification
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("ssg.notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }

        /// <summary>
        /// Access denied view
        /// </summary>
        /// <returns>Access denied view</returns>
        protected ActionResult AccessDeniedView()
        {
            //return new HttpUnauthorizedResult();
            return RedirectToAction("AccessDenied", "Security", new { pageUrl = this.Request.RawUrl });
        }

        protected ActionResult AccessDenied()
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            if (workContext.CurrentUser.IsRegistered())
            {
                return RedirectToAction("AccessDenied", "SecurityAccess", new { pageUrl = this.Request.RawUrl });
            }
            else
            {
                return RedirectToAction("Login", "User", new { checkoutAsGuest = false, returnUrl = this.Request.RawUrl });
            }
        }

        protected ActionResult ResourceNotFound(string errorMsg)
        {
            return RedirectToAction("ResourceNotFound", "Error", new { pageUrl = this.Request.RawUrl, errorMessage = errorMsg });
        }

        protected ActionResult AccessDeniedPage(string errorMsg)
        {
            return RedirectToAction("AcessDenied", "Error", new { pageUrl = this.Request.RawUrl, errorMessage = errorMsg });
        }
    }
}
