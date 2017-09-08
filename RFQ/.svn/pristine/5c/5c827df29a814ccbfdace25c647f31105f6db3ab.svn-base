using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSG.Services.Security;
using SSG.Core;

namespace SSG.Web.Controllers
{
    public partial class MaintenanceController : BaseSSGController
    {
        #region fields

        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;

        #endregion

        #region ctor

        public MaintenanceController(
            IPermissionService permissionService,
            IWorkContext workContext
            )
        {
            this._permissionService = permissionService;
            this._workContext = workContext;
        }

        #endregion

        public virtual ActionResult Index()
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin)) 
                return AccessDenied();

            return View();
        }
    }
}
