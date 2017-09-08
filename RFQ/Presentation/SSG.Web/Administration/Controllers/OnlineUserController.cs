using System;
using System.Linq;
using System.Web.Mvc;
using SSG.Admin.Models.Users;
using SSG.Core.Domain.Common;
using SSG.Core.Domain.Users;
using SSG.Services.Common;
using SSG.Services.Users;
using SSG.Services.Directory;
using SSG.Services.Helpers;
using SSG.Services.Localization;
using SSG.Services.Security;
using SSG.Web.Framework.Controllers;
using Telerik.Web.Mvc;

namespace SSG.Admin.Controllers
{
    [AdminAuthorize]
    public partial class OnlineUserController : BaseSSGController
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IGeoCountryLookup _geoCountryLookup;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly UserSettings _userSettings;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Constructors

        public OnlineUserController(IUserService userService,
            IGeoCountryLookup geoCountryLookup, IDateTimeHelper dateTimeHelper,
            UserSettings userSettings, AdminAreaSettings adminAreaSettings,
            IPermissionService permissionService, ILocalizationService localizationService)
        {
            this._userService = userService;
            this._geoCountryLookup = geoCountryLookup;
            this._dateTimeHelper = dateTimeHelper;
            this._userSettings = userSettings;
            this._adminAreaSettings = adminAreaSettings;
            this._permissionService = permissionService;
            this._localizationService = localizationService;
        }

        #endregion
        
        #region Online users

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var users = _userService.GetOnlineUsers(DateTime.UtcNow.AddMinutes(-_userSettings.OnlineUserMinutes),
                null, 0, _adminAreaSettings.GridPageSize);

            var model = new GridModel<OnlineUserModel>
            {
                Data = users.Select(x =>
                {
                    return new OnlineUserModel()
                    {
                        Id = x.Id,
                        UserInfo = x.IsRegistered() ? x.Email : _localizationService.GetResource("Admin.Users.Guest"),
                        LastIpAddress = x.LastIpAddress,
                        Location = _geoCountryLookup.LookupCountryName(x.LastIpAddress),
                        LastActivityDate = _dateTimeHelper.ConvertToUserTime(x.LastActivityDateUtc, DateTimeKind.Utc),
                        LastVisitedPage = x.GetAttribute<string>(SystemUserAttributeNames.LastVisitedPage)
                    };
                }),
                Total = users.TotalCount
            };
            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var users = _userService.GetOnlineUsers(DateTime.UtcNow.AddMinutes(-_userSettings.OnlineUserMinutes),
                null, command.Page - 1, command.PageSize);
            var model = new GridModel<OnlineUserModel>
            {
                Data = users.Select(x =>
                {
                    return new OnlineUserModel()
                    {
                        Id = x.Id,
                        UserInfo = x.IsRegistered() ? x.Email : _localizationService.GetResource("Admin.Users.Guest"),
                        LastIpAddress = x.LastIpAddress,
                        Location = _geoCountryLookup.LookupCountryName(x.LastIpAddress),
                        LastActivityDate = _dateTimeHelper.ConvertToUserTime(x.LastActivityDateUtc, DateTimeKind.Utc),
                        LastVisitedPage = x.GetAttribute<string>(SystemUserAttributeNames.LastVisitedPage)
                    };
                }),
                Total = users.TotalCount
            };
            return new JsonResult
            {
                Data = model
            };
        }

        #endregion
    }
}
