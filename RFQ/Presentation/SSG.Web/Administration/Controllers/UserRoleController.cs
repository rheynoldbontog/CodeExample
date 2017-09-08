using System;
using System.Linq;
using System.Web.Mvc;
using SSG.Admin.Models.Users;
using SSG.Core;
using SSG.Services.Users;
using SSG.Services.Localization;
using SSG.Services.Logging;
using SSG.Services.Security;
using SSG.Web.Framework.Controllers;
using Telerik.Web.Mvc;

namespace SSG.Admin.Controllers
{
	[AdminAuthorize]
    public partial class UserRoleController : BaseSSGController
	{
		#region Fields

		private readonly IUserService _userService;
        private readonly ILocalizationService _localizationService;
        private readonly IUserActivityService _userActivityService;
        private readonly IPermissionService _permissionService;

		#endregion

		#region Constructors

        public UserRoleController(IUserService userService,
            ILocalizationService localizationService, IUserActivityService userActivityService,
            IPermissionService permissionService)
		{
            this._userService = userService;
            this._localizationService = localizationService;
            this._userActivityService = userActivityService;
            this._permissionService = permissionService;
		}

		#endregion 

		#region User roles

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

		public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUserRoles))
                return AccessDeniedView();
            
			var userRoles = _userService.GetAllUserRoles(true);
			var gridModel = new GridModel<UserRoleModel>
			{
                Data = userRoles.Select(x => x.ToModel()),
                Total = userRoles.Count()
			};
			return View(gridModel);
		}

		[HttpPost, GridAction(EnableCustomBinding = true)]
		public ActionResult List(GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUserRoles))
                return AccessDeniedView();
            
            var userRoles = _userService.GetAllUserRoles(true);
            var gridModel = new GridModel<UserRoleModel>
			{
                Data = userRoles.Select(x => x.ToModel()),
                Total = userRoles.Count()
			};
			return new JsonResult
			{
				Data = gridModel
			};
		}

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUserRoles))
                return AccessDeniedView();
            
            var model = new UserRoleModel();
            //default values
            model.Active = true;
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        public ActionResult Create(UserRoleModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUserRoles))
                return AccessDeniedView();
            
            if (ModelState.IsValid)
            {
                var userRole = model.ToEntity();
                _userService.InsertUserRole(userRole);

                //activity log
                _userActivityService.InsertActivity("AddNewUserRole", _localizationService.GetResource("ActivityLog.AddNewUserRole"), userRole.Name);

                SuccessNotification(_localizationService.GetResource("Admin.Users.UserRoles.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = userRole.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

		public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUserRoles))
                return AccessDeniedView();
            
            var userRole = _userService.GetUserRoleById(id);
            if (userRole == null)
                //No user role found with the specified id
                return RedirectToAction("List");

            return View(userRole.ToModel());
		}

        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        public ActionResult Edit(UserRoleModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUserRoles))
                return AccessDeniedView();
            
            var userRole = _userService.GetUserRoleById(model.Id);
            if (userRole == null)
                //No user role found with the specified id
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    if (userRole.IsSystemRole && !model.Active)
                        throw new SSGException(_localizationService.GetResource("Admin.Users.UserRoles.Fields.Active.CantEditSystem"));

                    if (userRole.IsSystemRole && !userRole.SystemName.Equals(model.SystemName, StringComparison.InvariantCultureIgnoreCase))
                        throw new SSGException(_localizationService.GetResource("Admin.Users.UserRoles.Fields.SystemName.CantEditSystem"));


                    userRole = model.ToEntity(userRole);
                    _userService.UpdateUserRole(userRole);

                    //activity log
                    _userActivityService.InsertActivity("EditUserRole", _localizationService.GetResource("ActivityLog.EditUserRole"), userRole.Name);

                    SuccessNotification(_localizationService.GetResource("Admin.Users.UserRoles.Updated"));
                    return continueEditing ? RedirectToAction("Edit", userRole.Id) : RedirectToAction("List");
                }

                //If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = userRole.Id });
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUserRoles))
                return AccessDeniedView();
            
            var userRole = _userService.GetUserRoleById(id);
            if (userRole == null)
                //No user role found with the specified id
                return RedirectToAction("List");

            try
            {
                _userService.DeleteUserRole(userRole);

                //activity log
                _userActivityService.InsertActivity("DeleteUserRole", _localizationService.GetResource("ActivityLog.DeleteUserRole"), userRole.Name);

                SuccessNotification(_localizationService.GetResource("Admin.Users.UserRoles.Deleted"));
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = userRole.Id });
            }

		}

		#endregion
    }
}
