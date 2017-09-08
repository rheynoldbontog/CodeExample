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

namespace SSG.Web.Controllers
{
    public partial class RFQLineFormTypeController : BaseSSGController
    {
        #region Fields

        private readonly IRFQLineFormTypeService _rfqLineFormTypeService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;

        #endregion

        #region ctor

        public RFQLineFormTypeController(IRFQLineFormTypeService rfqLineFormTypeService,
            AdminAreaSettings adminAreaSettings,
            IWorkContext workContext,
            IPermissionService permissionService
            )
        {
            this._rfqLineFormTypeService = rfqLineFormTypeService;
            this._adminAreaSettings = adminAreaSettings;
            this._workContext = workContext;
            this._permissionService = permissionService;
        }

        #endregion

        #region Methods

        public virtual ActionResult Index(int? page)
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            page = page ?? 1;

            var query = this._rfqLineFormTypeService.GetAllRFQLineFormTypesQueryable();

            int totalCount = query.Count();

            query = query.OrderBy(s => s.DateUpdatedOnUtc).Skip((page.Value - 1) * _adminAreaSettings.GridPageSize).Take(_adminAreaSettings.GridPageSize);

            var supportCodes = from sc in query
                               select new RFQLineFormTypeModel
                               {
                                   Id = sc.Id,
                                   Name = sc.Name,
                                   Active = sc.Active
                               };

            var pagedList = new StaticPagedList<RFQLineFormTypeModel>(supportCodes, page.Value, _adminAreaSettings.GridPageSize, totalCount);

            if (Request.IsAjaxRequest())
            {
                return PartialView(MVC.RFQLineFormType.Views._Table, pagedList);
            }

            return View(pagedList);
        }

        public virtual ActionResult Create()
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var rfqLineFormType = new RFQLineFormTypeModel
            {
                Active = true
            };

            return View(MVC.RFQLineFormType.Views.CreateEdit, rfqLineFormType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(RFQLineFormTypeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rfqLineFormType = Mapper.Map<RFQLineFormType>(model);

                    rfqLineFormType.CreatedByUserId = _workContext.CurrentUser.Id;
                    rfqLineFormType.UpdatedByUserId = _workContext.CurrentUser.Id;
                    rfqLineFormType.DateCreatedOnUtc = DateTime.UtcNow;
                    rfqLineFormType.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._rfqLineFormTypeService.SaveRFQLineFormType(rfqLineFormType);
                    SuccessNotification("RFQ Line Form Type has been created.");
                    return RedirectToAction(MVC.RFQLineFormType.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.RFQLineFormType.Views.CreateEdit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var entity = this._rfqLineFormTypeService.GetRFQLineFormTypeById(id);

            var department = Mapper.Map<RFQLineFormTypeModel>(entity);

            return View(MVC.RFQLineFormType.Views.CreateEdit, department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(RFQLineFormTypeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rfqLineFormType = Mapper.Map<RFQLineFormType>(model);
                    rfqLineFormType.UpdatedByUserId = _workContext.CurrentUser.Id;
                    rfqLineFormType.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._rfqLineFormTypeService.SaveRFQLineFormType(rfqLineFormType);
                    SuccessNotification("RFQ Line Form Type has been saved.");
                    return RedirectToAction(MVC.RFQLineFormType.Index());
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