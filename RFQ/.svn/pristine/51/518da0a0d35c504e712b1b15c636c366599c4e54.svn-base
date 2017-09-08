using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SSG.Admin.Models.Users;
using SSG.Admin.Models.Security;
using SSG.Core;
using SSG.Services.Users;
using SSG.Services.Localization;
using SSG.Services.Logging;
using SSG.Services.Security;
using SSG.Web.Framework.Controllers;

namespace SSG.Admin.Controllers
{
	[AdminAuthorize]
    public partial class SecurityController : BaseSSGController
	{
		#region Fields

        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;
        private readonly IUserService _userService;
        private readonly ILocalizationService _localizationService;

		#endregion

		#region Constructors

        public SecurityController(ILogger logger, IWorkContext workContext,
            IPermissionService permissionService,
            IUserService userService, ILocalizationService localizationService)
		{
            this._logger = logger;
            this._workContext = workContext;
            this._permissionService = permissionService;
            this._userService = userService;
            this._localizationService = localizationService;
		}

		#endregion 

        #region Methods

        public ActionResult AccessDenied(string pageUrl)
        {
            var currentUser = _workContext.CurrentUser;
            if (currentUser == null || currentUser.IsGuest())
            {
                _logger.Information(string.Format("Access denied to anonymous request on {0}", pageUrl));
                return View();
            }

            _logger.Information(string.Format("Access denied to user #{0} '{1}' on {2}", currentUser.Email, currentUser.Email, pageUrl));


            return View();
        }

        public ActionResult Permissions()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAcl))
                return AccessDeniedView();

            var model = new PermissionMappingModel();

            var permissionRecords = _permissionService.GetAllPermissionRecords();
            var userRoles = _userService.GetAllUserRoles(true);
            foreach (var pr in permissionRecords)
            {
                model.AvailablePermissions.Add(new PermissionRecordModel()
                {
                    Name = pr.Name,
                    SystemName = pr.SystemName
                });
            }
            foreach (var cr in userRoles)
            {
                model.AvailableUserRoles.Add(new UserRoleModel()
                {
                    Id = cr.Id,
                    Name = cr.Name
                });
            }
            foreach (var pr in permissionRecords)
                foreach (var cr in userRoles)
                {
                    bool allowed = pr.UserRoles.Where(x => x.Id == cr.Id).ToList().Count() > 0;
                    if (!model.Allowed.ContainsKey(pr.SystemName))
                        model.Allowed[pr.SystemName] = new Dictionary<int, bool>();
                    model.Allowed[pr.SystemName][cr.Id] = allowed;
                }

            return View(model);
        }

        [HttpPost, ActionName("Permissions")]
        public ActionResult PermissionsSave(FormCollection form)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAcl))
                return AccessDeniedView();

            var permissionRecords = _permissionService.GetAllPermissionRecords();
            var userRoles = _userService.GetAllUserRoles(true);


            foreach (var cr in userRoles)
            {
                string formKey = "allow_" + cr.Id;
                var permissionRecordSystemNamesToRestrict = form[formKey] != null ? form[formKey].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();

                foreach (var pr in permissionRecords)
                {

                    bool allow = permissionRecordSystemNamesToRestrict.Contains(pr.SystemName);
                    if (allow)
                    {
                        if (pr.UserRoles.Where(x => x.Id == cr.Id).FirstOrDefault() == null)
                        {
                            pr.UserRoles.Add(cr);
                            _permissionService.UpdatePermissionRecord(pr);
                        }
                    }
                    else
                    {
                        if (pr.UserRoles.Where(x => x.Id == cr.Id).FirstOrDefault() != null)
                        {
                            pr.UserRoles.Remove(cr);
                            _permissionService.UpdatePermissionRecord(pr);
                        }
                    }
                }
            }

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.ACL.Updated"));
            return RedirectToAction("Permissions");
        }
        #endregion
    }
}
