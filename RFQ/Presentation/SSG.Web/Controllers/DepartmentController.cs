using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSG.Services.RFQ;
using SSG.Core.Domain.Common;
using SSG.Core;
using SSG.Services.Security;
using System.Web.Mvc;
using SSG.Web.Models.RFQ;
using PagedList;
using AutoMapper;
using SSG.Core.Domain.RFQ;
using SSG.Services.Users;

namespace SSG.Web.Controllers
{
    public partial class DepartmentController : BaseSSGController
    {
        #region Fields

        private readonly IDepartmentService _departmentService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;
        private readonly IUserService _userService;

        #endregion

        #region ctor

        public DepartmentController(IDepartmentService departmentService,
            AdminAreaSettings adminAreaSettings,
            IWorkContext workContext,
            IPermissionService permissionService,
            IUserService userService
            )
        {
            this._departmentService = departmentService;
            this._adminAreaSettings = adminAreaSettings;
            this._workContext = workContext;
            this._permissionService = permissionService;
            this._userService = userService;
        }

        #endregion

        #region Methods

        [HttpGet]
        public virtual JsonResult GetDepartmentByUserId(int userId)
        {
            var user = _userService.GetUserById(userId);
            var department = new Department();

            if (user != null && user.DepartmentId != null)
            {
                department = _departmentService.GetDepartmentById(user.DepartmentId.GetValueOrDefault());

                if (department != null)
                {
                    var defaultBuyer = department.GetDefaultBuyer();

                    return Json(new { success = true, name = department.Name, buyerId = defaultBuyer.Id }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                
            }
            else
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult Index(int? page)
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            page = page ?? 1;

            var query = this._departmentService.GetAllDepartmentsQueryable();

            int totalCount = query.Count();

            query = query.OrderBy(s => s.DateUpdatedOnUtc).Skip((page.Value - 1) * _adminAreaSettings.GridPageSize).Take(_adminAreaSettings.GridPageSize);

            var supportCodes = from sc in query
                               select new DepartmentModel
                               {
                                   Id = sc.Id,
                                   Name = sc.Name,
                                   Active = sc.Active
                               };

            var pagedList = new StaticPagedList<DepartmentModel>(supportCodes, page.Value, _adminAreaSettings.GridPageSize, totalCount);

            if (Request.IsAjaxRequest())
            {
                return PartialView(MVC.Department.Views._Table, pagedList);
            }

            return View(pagedList);
        }

        public virtual ActionResult Create()
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var department = new DepartmentModel
            {
                Active = true
            };

            return View(MVC.Department.Views.CreateEdit, department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(DepartmentModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var department = Mapper.Map<Department>(model);

                    department.CreatedByUserId = _workContext.CurrentUser.Id;
                    department.UpdatedByUserId = _workContext.CurrentUser.Id;
                    department.DateCreatedOnUtc = DateTime.UtcNow;
                    department.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._departmentService.SaveDepartment(department);
                    SuccessNotification("Department has been created.");
                    return RedirectToAction(MVC.Department.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.Department.Views.CreateEdit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var entity = this._departmentService.GetDepartmentById(id);

            var department = Mapper.Map<DepartmentModel>(entity);

            return View(MVC.Department.Views.CreateEdit, department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(DepartmentModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var department = Mapper.Map<Department>(model);
                    department.UpdatedByUserId = _workContext.CurrentUser.Id;
                    department.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._departmentService.SaveDepartment(department);
                    SuccessNotification("Department has been saved.");
                    return RedirectToAction(MVC.Department.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.Department.Views.CreateEdit, model);
        }

        #endregion

    }
}