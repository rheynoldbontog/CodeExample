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
    public partial class UOMController : BaseSSGController
    {
        #region Fields

        private readonly IUOMService _uomService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;

        #endregion

        #region ctor

        public UOMController(IUOMService uomService,
            AdminAreaSettings adminAreaSettings,
            IWorkContext workContext,
            IPermissionService permissionService
            )
        {
            this._uomService = uomService;
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

            var query = this._uomService.GetAllUOMsQueryable();

            int totalCount = query.Count();

            query = query.OrderBy(s => s.DateUpdatedOnUtc).Skip((page.Value - 1) * _adminAreaSettings.GridPageSize).Take(_adminAreaSettings.GridPageSize);

            var supportCodes = from sc in query
                               select new UOMModel
                               {
                                   Id = sc.Id,
                                   Name = sc.Name,
                                   Active = sc.Active
                               };

            var pagedList = new StaticPagedList<UOMModel>(supportCodes, page.Value, _adminAreaSettings.GridPageSize, totalCount);

            if (Request.IsAjaxRequest())
            {
                return PartialView(MVC.UOM.Views._Table, pagedList);
            }

            return View(pagedList);
        }

        public virtual ActionResult Create()
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var uom = new UOMModel
            {
                Active = true
            };

            return View(MVC.UOM.Views.CreateEdit, uom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(UOMModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var uom = Mapper.Map<UOM>(model);

                    uom.CreatedByUserId = _workContext.CurrentUser.Id;
                    uom.UpdatedByUserId = _workContext.CurrentUser.Id;
                    uom.DateCreatedOnUtc = DateTime.UtcNow;
                    uom.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._uomService.SaveUOM(uom);
                    SuccessNotification("Unit Of Measurement has been created.");
                    return RedirectToAction(MVC.UOM.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.UOM.Views.CreateEdit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var entity = this._uomService.GetUOMById(id);

            var uom = Mapper.Map<UOMModel>(entity);

            return View(MVC.UOM.Views.CreateEdit, uom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(UOMModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var uom = Mapper.Map<UOM>(model);
                    uom.UpdatedByUserId = _workContext.CurrentUser.Id;
                    uom.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._uomService.SaveUOM(uom);
                    SuccessNotification("Unit Of Measurement has been saved.");
                    return RedirectToAction(MVC.UOM.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.UOM.Views.CreateEdit, model);
        }

        #endregion
    }
}