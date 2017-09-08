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
    public partial class EquipmentPurchaseTypeController : BaseSSGController
    {
        #region Fields

        private readonly IEquipmentPurchaseTypeService _equipmentPurchaseTypeService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;

        #endregion

        #region ctor

        public EquipmentPurchaseTypeController(IEquipmentPurchaseTypeService equipmentPurchaseTypeService,
            AdminAreaSettings adminAreaSettings,
            IWorkContext workContext,
            IPermissionService permissionService
            )
        {
            this._equipmentPurchaseTypeService = equipmentPurchaseTypeService;
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

            var query = this._equipmentPurchaseTypeService.GetAllEquipmentPurchaseTypeQueryable();

            int totalCount = query.Count();

            query = query.OrderBy(s => s.DateUpdatedOnUtc).Skip((page.Value - 1) * _adminAreaSettings.GridPageSize).Take(_adminAreaSettings.GridPageSize);

            var supportCodes = from sc in query
                               select new EquipmentPurchaseTypeModel
                               {
                                   Id = sc.Id,
                                   Name = sc.Name,
                                   Active = sc.Active
                               };

            var pagedList = new StaticPagedList<EquipmentPurchaseTypeModel>(supportCodes, page.Value, _adminAreaSettings.GridPageSize, totalCount);

            if (Request.IsAjaxRequest())
            {
                return PartialView(MVC.EquipmentPurchaseType.Views._Table, pagedList);
            }

            return View(pagedList);
        }

        public virtual ActionResult Create()
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var equipmentCalibrationType = new EquipmentPurchaseTypeModel
            {
                Active = true
            };

            return View(MVC.EquipmentPurchaseType.Views.CreateEdit, equipmentCalibrationType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(EquipmentPurchaseTypeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var equipmentPurchaseType = Mapper.Map<EquipmentPurchaseType>(model);

                    equipmentPurchaseType.CreatedByUserId = _workContext.CurrentUser.Id;
                    equipmentPurchaseType.UpdatedByUserId = _workContext.CurrentUser.Id;
                    equipmentPurchaseType.DateCreatedOnUtc = DateTime.UtcNow;
                    equipmentPurchaseType.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._equipmentPurchaseTypeService.SaveEquipmentPurchaseType(equipmentPurchaseType);
                    SuccessNotification("Equipment Purchase Type has been created.");
                    return RedirectToAction(MVC.EquipmentPurchaseType.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.EquipmentPurchaseType.Views.CreateEdit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var entity = this._equipmentPurchaseTypeService.GetEquipmentPurchaseTypeById(id);

            var equipmentPurchaseType = Mapper.Map<EquipmentPurchaseTypeModel>(entity);

            return View(MVC.EquipmentPurchaseType.Views.CreateEdit, equipmentPurchaseType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(EquipmentPurchaseTypeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var equipmentPurchaseType = Mapper.Map<EquipmentPurchaseType>(model);
                    equipmentPurchaseType.UpdatedByUserId = _workContext.CurrentUser.Id;
                    equipmentPurchaseType.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._equipmentPurchaseTypeService.SaveEquipmentPurchaseType(equipmentPurchaseType);
                    SuccessNotification("Equipment Purchase Type has been saved.");
                    return RedirectToAction(MVC.EquipmentPurchaseType.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.EquipmentPurchaseType.Views.CreateEdit, model);
        }

        #endregion
    }
}