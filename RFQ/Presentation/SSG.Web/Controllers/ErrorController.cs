using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSG.Core;
using SSG.Services.Security;
using System.Web.Mvc;
using SSG.Services.Logging;

namespace SSG.Web.Controllers
{
    public partial class ErrorController : BaseSSGController
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;
        
        #endregion

        #region ctor

        public ErrorController(ILogger logger, 
            IWorkContext workContext,
            IPermissionService permissionService)
        {
            this._logger = logger;
            this._workContext = workContext;
            this._permissionService = permissionService;
        }

        #endregion

        #region Methods

        public virtual ActionResult ResourceNotFound(string pageUrl, string errorMessage)
        {
            var currentUser = _workContext.CurrentUser;

            ViewBag.pageUrl = pageUrl;
            ViewBag.ErrorMessage = errorMessage;

            _logger.Information(string.Format("ResourceNotFound: {0} '{1}' on {2}", errorMessage, currentUser.Email, pageUrl));
            
            return View();
        }

        public virtual ActionResult AcessDenied(string pageUrl, string errorMessage)
        {
            var currentUser = _workContext.CurrentUser;

            ViewBag.pageUrl = pageUrl;
            ViewBag.ErrorMessage = errorMessage;

            _logger.Information(string.Format("Access Denied: {0} '{1}' on {2}", errorMessage, currentUser.Email, pageUrl));

            return View();
        }

        #endregion
    }
}