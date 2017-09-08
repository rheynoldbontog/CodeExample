using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SSG.Admin.Models.Common;
using SSG.Admin.Models.Users;
using SSG.Core;
using SSG.Core.Domain.Common;
using SSG.Core.Domain.Directory;
using SSG.Core.Domain.Forums;
using SSG.Core.Domain.Messages;
using SSG.Core.Domain.Users;
using SSG.Services.Authentication.External;
using SSG.Services.Common;
using SSG.Services.Directory;
using SSG.Services.ExportImport;
using SSG.Services.Forums;
using SSG.Services.Helpers;
using SSG.Services.Localization;
using SSG.Services.Logging;
using SSG.Services.Messages;
using SSG.Services.Security;
using SSG.Services.Users;
using SSG.Web.Framework.Controllers;
using SSG.Web.Framework.Mvc;
using Telerik.Web.Mvc;
using SSG.Services.RFQ;
using SSG.Core.Domain.RFQ;

namespace SSG.Admin.Controllers
{
    [AdminAuthorize]
    public partial class UserController : BaseSSGController
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IUserReportService _userReportService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IDepartmentService _departmentService;
        private readonly IAddressService _addressService;
        private readonly UserSettings _userSettings;
        private readonly IWorkContext _workContext;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IExportManager _exportManager;
        private readonly IUserActivityService _userActivityService;
        private readonly IPermissionService _permissionService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IEmailAccountService _emailAccountService;
        private readonly ForumSettings _forumSettings;
        private readonly IForumService _forumService;
        private readonly IOpenAuthenticationService _openAuthenticationService;

        #endregion

        #region Constructors

        public UserController(IUserService userService,
            IGenericAttributeService genericAttributeService,
            IUserRegistrationService userRegistrationService,
            IUserReportService userReportService, IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService, DateTimeSettings dateTimeSettings,
            ICountryService countryService, IStateProvinceService stateProvinceService, IDepartmentService departmentService,
            IAddressService addressService,
            UserSettings userSettings,
            IWorkContext workContext, IPriceFormatter priceFormatter,
            IExportManager exportManager,
            IUserActivityService userActivityService,
            IPermissionService permissionService, AdminAreaSettings adminAreaSettings,
            IQueuedEmailService queuedEmailService, EmailAccountSettings emailAccountSettings,
            IEmailAccountService emailAccountService, ForumSettings forumSettings,
            IForumService forumService, IOpenAuthenticationService openAuthenticationService)
        {
            this._userService = userService;
            this._genericAttributeService = genericAttributeService;
            this._userRegistrationService = userRegistrationService;
            this._userReportService = userReportService;
            this._dateTimeHelper = dateTimeHelper;
            this._localizationService = localizationService;
            this._departmentService = departmentService;
            this._dateTimeSettings = dateTimeSettings;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._addressService = addressService;
            this._userSettings = userSettings;
            this._workContext = workContext;
            this._priceFormatter = priceFormatter;
            this._exportManager = exportManager;
            this._userActivityService = userActivityService;
            this._permissionService = permissionService;
            this._adminAreaSettings = adminAreaSettings;
            this._queuedEmailService = queuedEmailService;
            this._emailAccountSettings = emailAccountSettings;
            this._emailAccountService = emailAccountService;
            this._forumSettings = forumSettings;
            this._forumService = forumService;
            this._openAuthenticationService = openAuthenticationService;
        }

        #endregion

        #region Utilities

        [NonAction]
        protected string GetUserRolesNames(IList<UserRole> userRoles, string separator = ",")
        {
            var sb = new StringBuilder();
            for (int i = 0; i < userRoles.Count; i++)
            {
                sb.Append(userRoles[i].Name);
                if (i != userRoles.Count - 1)
                {
                    sb.Append(separator);
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }

        [NonAction]
        protected IList<RegisteredUserReportLineModel> GetReportRegisteredUsersModel()
        {
            var report = new List<RegisteredUserReportLineModel>();
            report.Add(new RegisteredUserReportLineModel()
            {
                Period = _localizationService.GetResource("Admin.Users.Reports.RegisteredUsers.Fields.Period.7days"),
                Users = _userReportService.GetRegisteredUsersReport(7)
            });

            report.Add(new RegisteredUserReportLineModel()
            {
                Period = _localizationService.GetResource("Admin.Users.Reports.RegisteredUsers.Fields.Period.14days"),
                Users = _userReportService.GetRegisteredUsersReport(14)
            });
            report.Add(new RegisteredUserReportLineModel()
            {
                Period = _localizationService.GetResource("Admin.Users.Reports.RegisteredUsers.Fields.Period.month"),
                Users = _userReportService.GetRegisteredUsersReport(30)
            });
            report.Add(new RegisteredUserReportLineModel()
            {
                Period = _localizationService.GetResource("Admin.Users.Reports.RegisteredUsers.Fields.Period.year"),
                Users = _userReportService.GetRegisteredUsersReport(365)
            });

            return report;
        }

        [NonAction]
        protected IList<UserModel.AssociatedExternalAuthModel> GetAssociatedExternalAuthRecords(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var result = new List<UserModel.AssociatedExternalAuthModel>();
            foreach (var record in _openAuthenticationService.GetExternalIdentifiersFor(user))
            {
                var method = _openAuthenticationService.LoadExternalAuthenticationMethodBySystemName(record.ProviderSystemName);
                if (method == null)
                    continue;

                result.Add(new UserModel.AssociatedExternalAuthModel()
                {
                    Id = record.Id,
                    Email = record.Email,
                    ExternalIdentifier = record.ExternalIdentifier,
                    AuthMethodName = method.PluginDescriptor.FriendlyName
                });
            }

            return result;
        }

        [NonAction]
        protected UserModel PrepareUserModelForList(User user)
        {
            return new UserModel()
            {
                Id = user.Id,
                Email = !String.IsNullOrEmpty(user.Email) ? user.Email : (user.IsGuest() ? _localizationService.GetResource("Admin.Users.Guest") : "Unknown"),
                Username = user.Username,
                FullName = user.GetFullName(),
                Company = user.GetAttribute<string>(SystemUserAttributeNames.Company),
                Phone = user.GetAttribute<string>(SystemUserAttributeNames.Phone),
                ZipPostalCode = user.GetAttribute<string>(SystemUserAttributeNames.ZipPostalCode),
                UserRoleNames = GetUserRolesNames(user.UserRoles.ToList()),
                Active = user.Active,
                CreatedOn = _dateTimeHelper.ConvertToUserTime(user.CreatedOnUtc, DateTimeKind.Utc),
                LastActivityDate = _dateTimeHelper.ConvertToUserTime(user.LastActivityDateUtc, DateTimeKind.Utc),
            };
        }

        [NonAction]
        protected string ValidateUserRoles(IList<UserRole> userRoles)
        {
            if (userRoles == null)
                throw new ArgumentNullException("userRoles");

            //ensure a user is not added to both 'Guests' and 'Registered' user roles
            //ensure that a user is in at least one required role ('Guests' and 'Registered')
            bool isInGuestsRole = userRoles
                .Where(cr => cr.SystemName == SystemUserRoleNames.Guests)
                .FirstOrDefault() != null;
            bool isInRegisteredRole =
                userRoles
                .Where(cr => cr.SystemName == SystemUserRoleNames.Registered)
                .FirstOrDefault() != null;
            if (isInGuestsRole && isInRegisteredRole)
                return "The user cannot be in both 'Guests' and 'Registered' user roles";
            if (!isInGuestsRole && !isInRegisteredRole)
                return "Add the user to 'Guests' or 'Registered' user role";

            //no errors
            return "";
        }

        #endregion

        #region Users

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //load registered users by default
            var defaultRoleIds = new[] {_userService.GetUserRoleBySystemName(SystemUserRoleNames.Registered).Id};
            var listModel = new UserListModel()
            {
                UsernamesEnabled = _userSettings.UsernamesEnabled,
                DateOfBirthEnabled = _userSettings.DateOfBirthEnabled,
                CompanyEnabled = _userSettings.CompanyEnabled,
                PhoneEnabled = _userSettings.PhoneEnabled,
                ZipPostalCodeEnabled = _userSettings.ZipPostalCodeEnabled,
                AvailableUserRoles = _userService.GetAllUserRoles(true).Select(cr => cr.ToModel()).ToList(),
                SearchUserRoleIds = defaultRoleIds,
            };
            var users = _userService.GetAllUsers(null, null, defaultRoleIds, null,
                null, null, null, 0, 0, null, null, null,
                0, _adminAreaSettings.GridPageSize);
            //user list
            listModel.Users = new GridModel<UserModel>
            {
                Data = users.Select(PrepareUserModelForList),
                Total = users.TotalCount
            };
            return View(listModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult UserList(GridCommand command, UserListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var searchDayOfBirth = 0;
            int searchMonthOfBirth = 0;
            if (!String.IsNullOrWhiteSpace(model.SearchDayOfBirth))
                searchDayOfBirth = Convert.ToInt32(model.SearchDayOfBirth);
            if (!String.IsNullOrWhiteSpace(model.SearchMonthOfBirth))
                searchMonthOfBirth = Convert.ToInt32(model.SearchMonthOfBirth);

            var users = _userService.GetAllUsers(null, null,
                model.SearchUserRoleIds, model.SearchEmail, model.SearchUsername,
                model.SearchFirstName, model.SearchLastName,
                searchDayOfBirth, searchMonthOfBirth,
                model.SearchCompany, model.SearchPhone, model.SearchZipPostalCode,
                command.Page - 1, command.PageSize);
            var gridModel = new GridModel<UserModel>
            {
                Data = users.Select(PrepareUserModelForList),
                Total = users.TotalCount
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        
        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var model = new UserModel();
            model.UsernamesEnabled = _userSettings.UsernamesEnabled;
            model.AllowUsersToChangeUsernames = _userSettings.AllowUsersToChangeUsernames;
            model.AllowUsersToSetTimeZone = _dateTimeSettings.AllowUsersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem() { Text = tzi.DisplayName, Value = tzi.Id, Selected = (tzi.Id == _dateTimeHelper.DefaultSiteTimeZone.Id) });
            //user roles
            model.AvailableUserRoles = _userService
                .GetAllUserRoles(true)
                .Select(cr => cr.ToModel())
                .ToList();
            model.SelectedUserRoleIds = new int[0];
            model.AllowManagingUserRoles = _permissionService.Authorize(StandardPermissionProvider.ManageUserRoles);
            //form fields
            model.GenderEnabled = _userSettings.GenderEnabled;
            model.DateOfBirthEnabled = _userSettings.DateOfBirthEnabled;
            model.CompanyEnabled = _userSettings.CompanyEnabled;
            model.StreetAddressEnabled = _userSettings.StreetAddressEnabled;
            model.StreetAddress2Enabled = _userSettings.StreetAddress2Enabled;
            model.ZipPostalCodeEnabled = _userSettings.ZipPostalCodeEnabled;
            model.CityEnabled = _userSettings.CityEnabled;
            model.CountryEnabled = _userSettings.CountryEnabled;
            model.StateProvinceEnabled = _userSettings.StateProvinceEnabled;
            model.PhoneEnabled = _userSettings.PhoneEnabled;
            model.FaxEnabled = _userSettings.FaxEnabled;
            model.DepartmentEnabled = _userSettings.DepartmentEnabled;

            //departments
            if (_userSettings.DepartmentEnabled)
            {
                foreach (var d in _departmentService.GetAllDepartments())
                {
                    model.AvailableDepartments.Add(new SelectListItem() { Text = d.Name, Value = d.Id.ToString() });
                }
            }

            if (_userSettings.CountryEnabled)
            {
                model.AvailableCountries.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Address.SelectCountry"), Value = "0" });
                foreach (var c in _countryService.GetAllCountries())
                {
                    model.AvailableCountries.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });
                }

                if (_userSettings.StateProvinceEnabled)
                {
                    //states
                    var states = _stateProvinceService.GetStateProvincesByCountryId(model.CountryId).ToList();
                    if (states.Count > 0)
                    {
                        foreach (var s in states)
                            model.AvailableStates.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString(), Selected = (s.Id == model.StateProvinceId) });
                    }
                    else
                        model.AvailableStates.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Address.OtherNonUS"), Value = "0" });

                }
            }

            //default value
            model.Active = true;

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(UserModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            if (!String.IsNullOrWhiteSpace(model.Email))
            {
                var cust2 = _userService.GetUserByEmail(model.Email);
                if (cust2 != null)
                    ModelState.AddModelError("", "Email is already registered");
            }
            if (!String.IsNullOrWhiteSpace(model.Username) & _userSettings.UsernamesEnabled)
            {
                var cust2 = _userService.GetUserByEmail(model.Username);
                if (cust2 != null)
                    ModelState.AddModelError("", "Username is already registered");
            }

            //validate user roles
            var allUserRoles = _userService.GetAllUserRoles(true);
            var newUserRoles = new List<UserRole>();
            foreach (var userRole in allUserRoles)
                if (model.SelectedUserRoleIds != null && model.SelectedUserRoleIds.Contains(userRole.Id))
                    newUserRoles.Add(userRole);
            var userRolesError = ValidateUserRoles(newUserRoles);
            if (!String.IsNullOrEmpty(userRolesError))
                ModelState.AddModelError("", userRolesError);
            bool allowManagingUserRoles = _permissionService.Authorize(StandardPermissionProvider.ManageUserRoles);
            
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    UserGuid = Guid.NewGuid(),
                    Email = model.Email,
                    Username = model.Username,
                    AdminComment = model.AdminComment,
                    EmployeeId = model.EmployeeId,
                    TimeZoneId = model.TimeZoneId,
                    Active = model.Active,
                    CreatedOnUtc = DateTime.UtcNow,
                    LastActivityDateUtc = DateTime.UtcNow,
                };

                //department
                if (_userSettings.DepartmentEnabled)
                {
                    user.DepartmentId = model.DepartmentId;
                }

                _userService.InsertUser(user);
                
                //form fields
                if (_userSettings.GenderEnabled)
                    _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.Gender, model.Gender);
                _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.FirstName, model.FirstName);
                _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.LastName, model.LastName);
                if (_userSettings.DateOfBirthEnabled)
                    _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.DateOfBirth, model.DateOfBirth);
                if (_userSettings.CompanyEnabled)
                    _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.Company, model.Company);
                if (_userSettings.StreetAddressEnabled)
                    _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.StreetAddress, model.StreetAddress);
                if (_userSettings.StreetAddress2Enabled)
                    _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.StreetAddress2, model.StreetAddress2);
                if (_userSettings.ZipPostalCodeEnabled)
                    _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.ZipPostalCode, model.ZipPostalCode);
                if (_userSettings.CityEnabled)
                    _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.City, model.City);
                if (_userSettings.CountryEnabled)
                    _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.CountryId, model.CountryId);
                if (_userSettings.CountryEnabled && _userSettings.StateProvinceEnabled)
                    _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.StateProvinceId, model.StateProvinceId);
                if (_userSettings.PhoneEnabled)
                    _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.Phone, model.Phone);
                if (_userSettings.FaxEnabled)
                    _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.Fax, model.Fax);


                //password
                if (!String.IsNullOrWhiteSpace(model.Password))
                {
                    var changePassRequest = new ChangePasswordRequest(model.Email, false, _userSettings.DefaultPasswordFormat, model.Password);
                    var changePassResult = _userRegistrationService.ChangePassword(changePassRequest);
                    if (!changePassResult.Success)
                    {
                        foreach (var changePassError in changePassResult.Errors)
                            ErrorNotification(changePassError);
                    }
                }

                //user roles
                if (allowManagingUserRoles)
                {
                    foreach (var userRole in newUserRoles)
                        user.UserRoles.Add(userRole);
                    _userService.UpdateUser(user);
                }

                //activity log
                _userActivityService.InsertActivity("AddNewUser", _localizationService.GetResource("ActivityLog.AddNewUser"), user.Id);

                SuccessNotification(_localizationService.GetResource("Admin.Users.Users.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = user.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            model.UsernamesEnabled = _userSettings.UsernamesEnabled;
            model.AllowUsersToChangeUsernames = _userSettings.AllowUsersToChangeUsernames;
            model.AllowUsersToSetTimeZone = _dateTimeSettings.AllowUsersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem() { Text = tzi.DisplayName, Value = tzi.Id, Selected = (tzi.Id == model.TimeZoneId) });
            //user roles
            model.AvailableUserRoles = _userService
                .GetAllUserRoles(true)
                .Select(cr => cr.ToModel())
                .ToList();
            model.AllowManagingUserRoles = allowManagingUserRoles;
            //form fields
            model.GenderEnabled = _userSettings.GenderEnabled;
            model.DateOfBirthEnabled = _userSettings.DateOfBirthEnabled;
            model.CompanyEnabled = _userSettings.CompanyEnabled;
            model.StreetAddressEnabled = _userSettings.StreetAddressEnabled;
            model.StreetAddress2Enabled = _userSettings.StreetAddress2Enabled;
            model.ZipPostalCodeEnabled = _userSettings.ZipPostalCodeEnabled;
            model.CityEnabled = _userSettings.CityEnabled;
            model.CountryEnabled = _userSettings.CountryEnabled;
            model.StateProvinceEnabled = _userSettings.StateProvinceEnabled;
            model.PhoneEnabled = _userSettings.PhoneEnabled;
            model.FaxEnabled = _userSettings.FaxEnabled;
            model.DepartmentEnabled = _userSettings.DepartmentEnabled;

            //departments
            if (_userSettings.DepartmentEnabled)
            {
                foreach (var d in _departmentService.GetAllDepartments())
                {
                    model.AvailableDepartments.Add(new SelectListItem() { Text = d.Name, Value = d.Id.ToString() });
                }
            }

            if (_userSettings.CountryEnabled)
            {
                model.AvailableCountries.Add(new SelectListItem() { Text = _localizationService.GetResource("Address.SelectCountry"), Value = "0" });
                foreach (var c in _countryService.GetAllCountries())
                {
                    model.AvailableCountries.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString(), Selected = (c.Id == model.CountryId) });
                }


                if (_userSettings.StateProvinceEnabled)
                {
                    //states
                    var states = _stateProvinceService.GetStateProvincesByCountryId(model.CountryId).ToList();
                    if (states.Count > 0)
                    {
                        foreach (var s in states)
                            model.AvailableStates.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString(), Selected = (s.Id == model.StateProvinceId) });
                    }
                    else
                        model.AvailableStates.Add(new SelectListItem() { Text = _localizationService.GetResource("Address.OtherNonUS"), Value = "0" });

                }
            }
            return View(model);

        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var user = _userService.GetUserById(id);
            if (user == null || user.Deleted)
                //No user found with the specified id
                return RedirectToAction("List");

            var model = new UserModel();
            model.Id = user.Id;
            model.Email = user.Email;
            model.Username = user.Username;
            model.AdminComment = user.AdminComment;
            model.EmployeeId = user.EmployeeId;
            model.Active = user.Active;
            model.TimeZoneId = user.TimeZoneId;
            model.UsernamesEnabled = _userSettings.UsernamesEnabled;
            model.AllowUsersToChangeUsernames = _userSettings.AllowUsersToChangeUsernames;
            model.AllowUsersToSetTimeZone = _dateTimeSettings.AllowUsersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem() { Text = tzi.DisplayName, Value = tzi.Id, Selected = (tzi.Id == user.TimeZoneId) });
            model.CreatedOn = _dateTimeHelper.ConvertToUserTime(user.CreatedOnUtc, DateTimeKind.Utc);
            model.LastActivityDate = _dateTimeHelper.ConvertToUserTime(user.LastActivityDateUtc, DateTimeKind.Utc);
            model.LastIpAddress = user.LastIpAddress;
            model.LastVisitedPage = user.GetAttribute<string>(SystemUserAttributeNames.LastVisitedPage);
            
            //form fields
            model.FirstName = user.GetAttribute<string>(SystemUserAttributeNames.FirstName);
            model.LastName = user.GetAttribute<string>(SystemUserAttributeNames.LastName);
            model.Gender = user.GetAttribute<string>(SystemUserAttributeNames.Gender);
            model.DateOfBirth = user.GetAttribute<DateTime?>(SystemUserAttributeNames.DateOfBirth);
            model.Company = user.GetAttribute<string>(SystemUserAttributeNames.Company);
            model.StreetAddress = user.GetAttribute<string>(SystemUserAttributeNames.StreetAddress);
            model.StreetAddress2 = user.GetAttribute<string>(SystemUserAttributeNames.StreetAddress2);
            model.ZipPostalCode = user.GetAttribute<string>(SystemUserAttributeNames.ZipPostalCode);
            model.City = user.GetAttribute<string>(SystemUserAttributeNames.City);
            model.CountryId = user.GetAttribute<int>(SystemUserAttributeNames.CountryId);
            model.StateProvinceId = user.GetAttribute<int>(SystemUserAttributeNames.StateProvinceId);
            model.Phone = user.GetAttribute<string>(SystemUserAttributeNames.Phone);
            model.Fax = user.GetAttribute<string>(SystemUserAttributeNames.Fax);
            model.DepartmentId = user.DepartmentId;

            model.GenderEnabled = _userSettings.GenderEnabled;
            model.DateOfBirthEnabled = _userSettings.DateOfBirthEnabled;
            model.CompanyEnabled = _userSettings.CompanyEnabled;
            model.StreetAddressEnabled = _userSettings.StreetAddressEnabled;
            model.StreetAddress2Enabled = _userSettings.StreetAddress2Enabled;
            model.ZipPostalCodeEnabled = _userSettings.ZipPostalCodeEnabled;
            model.CityEnabled = _userSettings.CityEnabled;
            model.CountryEnabled = _userSettings.CountryEnabled;
            model.StateProvinceEnabled = _userSettings.StateProvinceEnabled;
            model.PhoneEnabled = _userSettings.PhoneEnabled;
            model.FaxEnabled = _userSettings.FaxEnabled;
            model.DepartmentEnabled = _userSettings.DepartmentEnabled;

            //departments
            if (_userSettings.DepartmentEnabled)
            {
                foreach (var d in _departmentService.GetAllDepartments())
                {
                    model.AvailableDepartments.Add(new SelectListItem() { Text = d.Name, Value = d.Id.ToString() });
                }
            }

            //countries and states
            if (_userSettings.CountryEnabled)
            {
                model.AvailableCountries.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Address.SelectCountry"), Value = "0" });
                foreach (var c in _countryService.GetAllCountries())
                {
                    model.AvailableCountries.Add(new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                        Selected = c.Id == model.CountryId
                    });
                }

                if (_userSettings.StateProvinceEnabled)
                {
                    //states
                    var states = _stateProvinceService.GetStateProvincesByCountryId(model.CountryId).ToList();
                    if (states.Count > 0)
                    {
                        foreach (var s in states)
                            model.AvailableStates.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString(), Selected = (s.Id == model.StateProvinceId) });
                    }
                    else
                        model.AvailableStates.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Address.OtherNonUS"), Value = "0" });

                }
            }

            //user roles
            model.AvailableUserRoles = _userService
                .GetAllUserRoles(true)
                .Select(cr => cr.ToModel())
                .ToList();
            model.SelectedUserRoleIds = user.UserRoles.Select(cr => cr.Id).ToArray();
            model.AllowManagingUserRoles = _permissionService.Authorize(StandardPermissionProvider.ManageUserRoles);

            //buyer departments
            if (user.IsBuyer(true))
            {
                model.AllowManageingBuyerDepartments = true;
                model.AvailableBuyerDepartments = _departmentService.GetAllDepartmentsQueryable(true).ToList();
                
                if (user.BuyerDepartments != null && user.BuyerDepartments.Count > 0)
                    model.SelectedBuyerDepartmentIds = user.BuyerDepartments.Select(cr => cr.Id).ToArray();

            }

            //external authentication records
            model.AssociatedExternalAuthRecords = GetAssociatedExternalAuthRecords(user);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormNameAttribute("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(UserModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var user = _userService.GetUserById(model.Id);
            if (user == null || user.Deleted)
                //No user found with the specified id
                return RedirectToAction("List");

            //validate user roles
            var allUserRoles = _userService.GetAllUserRoles(true);
            var newUserRoles = new List<UserRole>();
            foreach (var userRole in allUserRoles)
                if (model.SelectedUserRoleIds != null && model.SelectedUserRoleIds.Contains(userRole.Id))
                    newUserRoles.Add(userRole);
            var userRolesError = ValidateUserRoles(newUserRoles);
            if (!String.IsNullOrEmpty(userRolesError))
                ModelState.AddModelError("", userRolesError);
            bool allowManagingUserRoles = _permissionService.Authorize(StandardPermissionProvider.ManageUserRoles);
            
            var allBuyerDepartments = _departmentService.GetAllDepartmentsQueryable(true).ToList();
            bool allowManagingBuyerDepartments = _userSettings.DepartmentEnabled;

            if (ModelState.IsValid)
            {
                try
                {

                    user.AdminComment = model.AdminComment;
                    user.EmployeeId = model.EmployeeId;
                    user.TimeZoneId = model.TimeZoneId;
                    user.Active = model.Active;
                    //email
                    if (!String.IsNullOrWhiteSpace(model.Email))
                    {
                        _userRegistrationService.SetEmail(user, model.Email);
                    }
                    else
                    {
                        user.Email = model.Email;
                    }

                    //username
                    if (_userSettings.UsernamesEnabled && _userSettings.AllowUsersToChangeUsernames)
                    {
                        if (!String.IsNullOrWhiteSpace(model.Username))
                        {
                            _userRegistrationService.SetUsername(user, model.Username);
                        }
                        else
                        {
                            user.Username = model.Username;
                        }
                    }

                    _userService.UpdateUser(user);

                    //form fields
                    if (_userSettings.GenderEnabled)
                        _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.Gender, model.Gender);
                    _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.FirstName, model.FirstName);
                    _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.LastName, model.LastName);
                    if (_userSettings.DateOfBirthEnabled)
                        _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.DateOfBirth, model.DateOfBirth);
                    if (_userSettings.CompanyEnabled)
                        _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.Company, model.Company);
                    if (_userSettings.StreetAddressEnabled)
                        _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.StreetAddress, model.StreetAddress);
                    if (_userSettings.StreetAddress2Enabled)
                        _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.StreetAddress2, model.StreetAddress2);
                    if (_userSettings.ZipPostalCodeEnabled)
                        _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.ZipPostalCode, model.ZipPostalCode);
                    if (_userSettings.CityEnabled)
                        _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.City, model.City);
                    if (_userSettings.CountryEnabled)
                        _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.CountryId, model.CountryId);
                    if (_userSettings.CountryEnabled && _userSettings.StateProvinceEnabled)
                        _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.StateProvinceId, model.StateProvinceId);
                    if (_userSettings.PhoneEnabled)
                        _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.Phone, model.Phone);
                    if (_userSettings.FaxEnabled)
                        _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.Fax, model.Fax);

                    if (_userSettings.DepartmentEnabled)
                    {
                        user.DepartmentId = model.DepartmentId;
                        _userService.UpdateUser(user);
                    }

                    //user roles
                    if (allowManagingUserRoles)
                    {
                        foreach (var userRole in allUserRoles)
                        {
                            if (model.SelectedUserRoleIds != null && model.SelectedUserRoleIds.Contains(userRole.Id))
                            {
                                //new role
                                if (user.UserRoles.Where(cr => cr.Id == userRole.Id).Count() == 0)
                                    user.UserRoles.Add(userRole);
                            }
                            else
                            {
                                //removed role
                                if (user.UserRoles.Where(cr => cr.Id == userRole.Id).Count() > 0)
                                    user.UserRoles.Remove(userRole);
                            }
                        }
                        _userService.UpdateUser(user);
                    }

                    //buyer departments
                    if (allowManagingBuyerDepartments)
                    {
                        foreach (var buyerDepartment in allBuyerDepartments)
                        {
                            if (model.SelectedBuyerDepartmentIds != null && model.SelectedBuyerDepartmentIds.Contains(buyerDepartment.Id))
                            {
                                //new buyer department
                                if (user.BuyerDepartments.Where(cr => cr.Id == buyerDepartment.Id).Count() == 0)
                                   user.BuyerDepartments.Add(buyerDepartment);
                                else
                                {
                                    //remove buyer department
                                    if (user.BuyerDepartments.Where(cr => cr.Id == buyerDepartment.Id).Count() > 0)
                                        user.BuyerDepartments.Remove(buyerDepartment);
                                }
                            }
                        }
                    }

                    //activity log
                    _userActivityService.InsertActivity("EditUser", _localizationService.GetResource("ActivityLog.EditUser"), user.Id);

                    SuccessNotification(_localizationService.GetResource("Admin.Users.Users.Updated"));
                    return continueEditing ? RedirectToAction("Edit", user.Id) : RedirectToAction("List");
                }
                catch (Exception exc)
                {
                    ErrorNotification(exc.Message, false);
                }
            }


            //If we got this far, something failed, redisplay form
            model.UsernamesEnabled = _userSettings.UsernamesEnabled;
            model.AllowUsersToChangeUsernames = _userSettings.AllowUsersToChangeUsernames;
            model.AllowUsersToSetTimeZone = _dateTimeSettings.AllowUsersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem() { Text = tzi.DisplayName, Value = tzi.Id, Selected = (tzi.Id == model.TimeZoneId) });
            model.CreatedOn = _dateTimeHelper.ConvertToUserTime(user.CreatedOnUtc, DateTimeKind.Utc);
            model.LastActivityDate = _dateTimeHelper.ConvertToUserTime(user.LastActivityDateUtc, DateTimeKind.Utc);
            model.LastIpAddress = model.LastIpAddress;
            model.LastVisitedPage = user.GetAttribute<string>(SystemUserAttributeNames.LastVisitedPage);
            //form fields
            model.GenderEnabled = _userSettings.GenderEnabled;
            model.DateOfBirthEnabled = _userSettings.DateOfBirthEnabled;
            model.CompanyEnabled = _userSettings.CompanyEnabled;
            model.StreetAddressEnabled = _userSettings.StreetAddressEnabled;
            model.StreetAddress2Enabled = _userSettings.StreetAddress2Enabled;
            model.ZipPostalCodeEnabled = _userSettings.ZipPostalCodeEnabled;
            model.CityEnabled = _userSettings.CityEnabled;
            model.CountryEnabled = _userSettings.CountryEnabled;
            model.StateProvinceEnabled = _userSettings.StateProvinceEnabled;
            model.PhoneEnabled = _userSettings.PhoneEnabled;
            model.FaxEnabled = _userSettings.FaxEnabled;
            model.DepartmentEnabled = _userSettings.DepartmentEnabled;

            //departments
            if (_userSettings.DepartmentEnabled)
            {
                foreach (var d in _departmentService.GetAllDepartments())
                {
                    model.AvailableDepartments.Add(new SelectListItem() { Text = d.Name, Value = d.Id.ToString() });
                }
            }


            //countries and states
            if (_userSettings.CountryEnabled)
            {
                model.AvailableCountries.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Address.SelectCountry"), Value = "0" });
                foreach (var c in _countryService.GetAllCountries())
                {
                    model.AvailableCountries.Add(new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                        Selected = c.Id == model.CountryId
                    });
                }

                if (_userSettings.StateProvinceEnabled)
                {
                    //states
                    var states = _stateProvinceService.GetStateProvincesByCountryId(model.CountryId).ToList();
                    if (states.Count > 0)
                    {
                        foreach (var s in states)
                            model.AvailableStates.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString(), Selected = (s.Id == model.StateProvinceId) });
                    }
                    else
                        model.AvailableStates.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Address.OtherNonUS"), Value = "0" });

                }
            }
            //user roles
            model.AvailableUserRoles = _userService
                .GetAllUserRoles(true)
                .Select(cr => cr.ToModel())
                .ToList();
            model.AllowManagingUserRoles = allowManagingUserRoles;
            //external authentication records
            model.AssociatedExternalAuthRecords = GetAssociatedExternalAuthRecords(user);
            return View(model);
        }
        
        [HttpPost, ActionName("Edit")]
        [FormValueRequired("changepassword")]
        public ActionResult ChangePassword(UserModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var user = _userService.GetUserById(model.Id);
            if (user == null)
                //No user found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var changePassRequest = new ChangePasswordRequest(model.Email,
                    false, _userSettings.DefaultPasswordFormat, model.Password);
                var changePassResult = _userRegistrationService.ChangePassword(changePassRequest);
                if (changePassResult.Success)
                    SuccessNotification(_localizationService.GetResource("Admin.Users.Users.PasswordChanged"));
                else
                    foreach (var error in changePassResult.Errors)
                        ErrorNotification(error);
            }

            return RedirectToAction("Edit", user.Id);
        }
        
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var user = _userService.GetUserById(id);
            if (user == null)
                //No user found with the specified id
                return RedirectToAction("List");

            try
            {
                _userService.DeleteUser(user);

                //activity log
                _userActivityService.InsertActivity("DeleteUser", _localizationService.GetResource("ActivityLog.DeleteUser"), user.Id);

                SuccessNotification(_localizationService.GetResource("Admin.Users.Users.Deleted"));
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = user.Id });
            }
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("impersonate")]
        public ActionResult Impersonate(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.AllowUserImpersonation))
                return AccessDeniedView();

            var user = _userService.GetUserById(id);
            if (user == null)
                //No user found with the specified id
                return RedirectToAction("List");

            _genericAttributeService.SaveAttribute<int?>(_workContext.CurrentUser,
                SystemUserAttributeNames.ImpersonatedUserId, user.Id);

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public ActionResult SendEmail(UserModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var user = _userService.GetUserById(model.Id);
            if (user == null)
                //No user found with the specified id
                return RedirectToAction("List");

            try
            {
                if (String.IsNullOrWhiteSpace(user.Email))
                    throw new SSGException("User email is empty");
                if (!CommonHelper.IsValidEmail(user.Email))
                    throw new SSGException("User email is not valid");
                if (String.IsNullOrWhiteSpace(model.SendEmail.Subject))
                    throw new SSGException("Email subject is empty");
                if (String.IsNullOrWhiteSpace(model.SendEmail.Body))
                    throw new SSGException("Email body is empty");

                var emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
                if (emailAccount == null)
                    emailAccount = _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
                if (emailAccount == null)
                    throw new SSGException("Email account can't be loaded");

                var email = new QueuedEmail()
                {
                    EmailAccountId = emailAccount.Id,
                    FromName = emailAccount.DisplayName,
                    From = emailAccount.Email,
                    ToName = user.GetFullName(),
                    To = user.Email,
                    Subject = model.SendEmail.Subject,
                    Body = model.SendEmail.Body,
                    CreatedOnUtc = DateTime.UtcNow,
                };
                _queuedEmailService.InsertQueuedEmail(email);
                SuccessNotification(_localizationService.GetResource("Admin.Users.Users.SendEmail.Queued"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
            }

            return RedirectToAction("Edit", new { id = user.Id });
        }

        public ActionResult SendPm(UserModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var user = _userService.GetUserById(model.Id);
            if (user == null)
                //No user found with the specified id
                return RedirectToAction("List");

            try
            {
                if (!_forumSettings.AllowPrivateMessages)
                    throw new SSGException("Private messages are disabled");
                if (user.IsGuest())
                    throw new SSGException("User should be registered");
                if (String.IsNullOrWhiteSpace(model.SendPm.Subject))
                    throw new SSGException("PM subject is empty");
                if (String.IsNullOrWhiteSpace(model.SendPm.Message))
                    throw new SSGException("PM message is empty");


                var privateMessage = new PrivateMessage
                {
                    ToUserId = user.Id,
                    FromUserId = _workContext.CurrentUser.Id,
                    Subject = model.SendPm.Subject,
                    Text = model.SendPm.Message,
                    IsDeletedByAuthor = false,
                    IsDeletedByRecipient = false,
                    IsRead = false,
                    CreatedOnUtc = DateTime.UtcNow
                };

                _forumService.InsertPrivateMessage(privateMessage);
                SuccessNotification(_localizationService.GetResource("Admin.Users.Users.SendPM.Sent"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
            }

            return RedirectToAction("Edit", new { id = user.Id });
        }
        
        #endregion
       
        #region Addresses

        [GridAction]
        public ActionResult AddressesSelect(int userId, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var user = _userService.GetUserById(userId);
            if (user == null)
                throw new ArgumentException("No user found with the specified id", "userId");

            var addresses = user.Addresses.OrderByDescending(a => a.CreatedOnUtc).ThenByDescending(a => a.Id).ToList();
            var gridModel = new GridModel<AddressModel>
            {
                Data = addresses.Select(x =>
                {
                    var model = x.ToModel();
                    if (x.Country != null)
                        model.CountryName = x.Country.Name;
                    if (x.StateProvince != null)
                        model.StateProvinceName = x.StateProvince.Name;
                    model.AddressHtml = string.Format("<div>{0}<br />{1}<br />{2}<br />{3}, {4}, {5}<br />{6}<br /></div>", 
                        Server.HtmlEncode(model.Company),
                        Server.HtmlEncode(model.Address1),
                        Server.HtmlEncode(model.Address2),
                        Server.HtmlEncode(model.City),
                        Server.HtmlEncode(model.StateProvinceName),
                        Server.HtmlEncode(model.ZipPostalCode),
                        Server.HtmlEncode(model.CountryName));
                    return model;
                }),
                Total = addresses.Count
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        [GridAction]
        public ActionResult AddressDelete(int userId, int addressId, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var user = _userService.GetUserById(userId);
            if (user == null)
                throw new ArgumentException("No user found with the specified id", "userId");

            var address = user.Addresses.Where(a => a.Id == addressId).FirstOrDefault();
            user.RemoveAddress(address);
            _userService.UpdateUser(user);
            //now delete the address record
            _addressService.DeleteAddress(address);

            return AddressesSelect(userId, command);
        }
        
        public ActionResult AddressCreate(int userId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var user = _userService.GetUserById(userId);
            if (user == null)
                //No user found with the specified id
                return RedirectToAction("List");

            var model = new UserAddressModel();
            model.Address = new AddressModel();
            model.UserId = userId;
            //countries
            model.Address.AvailableCountries.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Address.SelectCountry"), Value = "0" });
            foreach (var c in _countryService.GetAllCountries(true))
                model.Address.AvailableCountries.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });
            model.Address.AvailableStates.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Address.OtherNonUS"), Value = "0" });

            return View(model);
        }

        [HttpPost]
        public ActionResult AddressCreate(UserAddressModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var user = _userService.GetUserById(model.UserId);
            if (user == null)
                //No user found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var address = model.Address.ToEntity();
                address.CreatedOnUtc = DateTime.UtcNow;
                //some validation
                if (address.CountryId == 0)
                    address.CountryId = null;
                if (address.StateProvinceId == 0)
                    address.StateProvinceId = null;
                user.Addresses.Add(address);
                _userService.UpdateUser(user);

                SuccessNotification(_localizationService.GetResource("Admin.Users.Users.Addresses.Added"));
                return RedirectToAction("AddressEdit", new { addressId = address.Id, userId = model.UserId });
            }

            //If we got this far, something failed, redisplay form
            model.UserId = user.Id;
            //countries
            model.Address.AvailableCountries.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Address.SelectCountry"), Value = "0" });
            foreach (var c in _countryService.GetAllCountries(true))
                model.Address.AvailableCountries.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString(), Selected = (c.Id == model.Address.CountryId) });
            //states
            var states = model.Address.CountryId.HasValue ? _stateProvinceService.GetStateProvincesByCountryId(model.Address.CountryId.Value, true).ToList() : new List<StateProvince>();
            if (states.Count > 0)
            {
                foreach (var s in states)
                    model.Address.AvailableStates.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString(), Selected = (s.Id == model.Address.StateProvinceId) });
            }
            else
                model.Address.AvailableStates.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Address.OtherNonUS"), Value = "0" });
            return View(model);
        }

        public ActionResult AddressEdit(int addressId, int userId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var user = _userService.GetUserById(userId);
            if (user == null)
                //No user found with the specified id
                return RedirectToAction("List");

            var address = _addressService.GetAddressById(addressId);
            if (address == null)
                //No address found with the specified id
                return RedirectToAction("Edit", new { id = user.Id });

            var model = new UserAddressModel();
            model.UserId = userId;
            model.Address = address.ToModel();
            //countries
            model.Address.AvailableCountries.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Address.SelectCountry"), Value = "0" });
            foreach (var c in _countryService.GetAllCountries(true))
                model.Address.AvailableCountries.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString(), Selected = (c.Id == address.CountryId) });
            //states
            var states = address.Country != null ? _stateProvinceService.GetStateProvincesByCountryId(address.Country.Id, true).ToList() : new List<StateProvince>();
            if (states.Count > 0)
            {
                foreach (var s in states)
                    model.Address.AvailableStates.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString(), Selected = (s.Id == address.StateProvinceId) });
            }
            else
                model.Address.AvailableStates.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Address.OtherNonUS"), Value = "0" });

            return View(model);
        }

        [HttpPost]
        public ActionResult AddressEdit(UserAddressModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var user = _userService.GetUserById(model.UserId);
            if (user == null)
                //No user found with the specified id
                return RedirectToAction("List");

            var address = _addressService.GetAddressById(model.Address.Id);
            if (address == null)
                //No address found with the specified id
                return RedirectToAction("Edit", new { id = user.Id });

            if (ModelState.IsValid)
            {
                address = model.Address.ToEntity(address);
                _addressService.UpdateAddress(address);

                SuccessNotification(_localizationService.GetResource("Admin.Users.Users.Addresses.Updated"));
                return RedirectToAction("AddressEdit", new { addressId = model.Address.Id, userId = model.UserId });
            }

            //If we got this far, something failed, redisplay form
            model.UserId = user.Id;
            model.Address = address.ToModel();
            //countries
            model.Address.AvailableCountries.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Address.SelectCountry"), Value = "0" });
            foreach (var c in _countryService.GetAllCountries(true))
                model.Address.AvailableCountries.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString(), Selected = (c.Id == address.CountryId) });
            //states
            var states = address.Country != null ? _stateProvinceService.GetStateProvincesByCountryId(address.Country.Id, true).ToList() : new List<StateProvince>();
            if (states.Count > 0)
            {
                foreach (var s in states)
                    model.Address.AvailableStates.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString(), Selected = (s.Id == address.StateProvinceId) });
            }
            else
                model.Address.AvailableStates.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Address.OtherNonUS"), Value = "0" });

            return View(model);
        }

        #endregion

        #region Reports

        public ActionResult Reports()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var model = new UserReportsModel();

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult ReportRegisteredUsers()
        {
            var model = GetReportRegisteredUsersModel();
            return PartialView(model);
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult ReportRegisteredUsersList(GridCommand command)
        {
            var model = GetReportRegisteredUsersModel();
            var gridModel = new GridModel<RegisteredUserReportLineModel>
            {
                Data = model,
                Total = model.Count
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        
        #endregion

        #region Activity log

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public JsonResult ListActivityLog(GridCommand command, int userId)
        {
            var activityLog = _userActivityService.GetAllActivities(null, null, userId, 0, command.Page - 1, command.PageSize);
            var gridModel = new GridModel<UserModel.ActivityLogModel>
            {
                Data = activityLog.Select(x =>
                {
                    var m = new UserModel.ActivityLogModel()
                    {
                        Id = x.Id,
                        ActivityLogTypeName = x.ActivityLogType.Name,
                        Comment = x.Comment,
                        CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc)
                    };
                    return m;

                }),
                Total = activityLog.TotalCount
            };
            return new JsonResult { Data = gridModel }; ;
        }

        #endregion

        #region Export / Import

        public ActionResult ExportExcelAll()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            try
            {
                var users = _userService.GetAllUsers(null, null, null, null,
                    null, null, null, 0, 0, null, null, null, 
                    0, int.MaxValue);

                string fileName = string.Format("users_{0}_{1}.xlsx", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), CommonHelper.GenerateRandomDigitCode(4));
                string filePath = System.IO.Path.Combine(Request.PhysicalApplicationPath, "content\\files\\ExportImport", fileName);

                _exportManager.ExportUsersToXlsx(filePath, users);

                var bytes = System.IO.File.ReadAllBytes(filePath);
                return File(bytes, "text/xls", fileName);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        public ActionResult ExportExcelSelected(string selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var users = new List<User>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();
                users.AddRange(_userService.GetUsersByIds(ids));
            }

            string fileName = string.Format("users_{0}_{1}.xlsx", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), CommonHelper.GenerateRandomDigitCode(4));
            string filePath = System.IO.Path.Combine(Request.PhysicalApplicationPath, "content\\files\\ExportImport", fileName);

            _exportManager.ExportUsersToXlsx(filePath, users);

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "text/xls", fileName);
        }

        public ActionResult ExportXmlAll()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            try
            {
                var users = _userService.GetAllUsers(null, null, null, null,
                    null, null, null, 0, 0, null, null, null, 
                    0, int.MaxValue);
                
                var fileName = string.Format("users_{0}.xml", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var xml = _exportManager.ExportUsersToXml(users);
                return new XmlDownloadResult(xml, fileName);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        public ActionResult ExportXmlSelected(string selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var users = new List<User>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();
                users.AddRange(_userService.GetUsersByIds(ids));
            }

            var fileName = string.Format("users_{0}.xml", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            var xml = _exportManager.ExportUsersToXml(users);
            return new XmlDownloadResult(xml, fileName);
        }

        #endregion
    }
}
