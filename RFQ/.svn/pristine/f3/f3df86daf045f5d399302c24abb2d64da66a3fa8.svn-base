using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSG.Core.Domain.Common;
using SSG.Core;
using SSG.Services.Security;
using System.Web.Mvc;
using SSG.Web.Models.RFQ;
using PagedList;
using AutoMapper;
using SSG.Core.Domain.RFQ;
using SSG.Services.RFQ;

namespace SSG.Web.Controllers
{
    public partial class EquipmentCalibrationTypeController : BaseSSGController
    {
        #region Fields

        private readonly IEquipmentCalibrationTypeService _equipmentCalibrationTypeService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;

        #endregion

        #region ctor

        public EquipmentCalibrationTypeController(IEquipmentCalibrationTypeService equipmentCalibrationTypeService,
            AdminAreaSettings adminAreaSettings,
            IWorkContext workContext,
            IPermissionService permissionService
            )
        {
            this._equipmentCalibrationTypeService = equipmentCalibrationTypeService;
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

            var query = this._equipmentCalibrationTypeService.GetAllEquipmentCalibrationTypeQueryable();

            int totalCount = query.Count();

            query = query.OrderBy(s => s.DateUpdatedOnUtc).Skip((page.Value - 1) * _adminAreaSettings.GridPageSize).Take(_adminAreaSettings.GridPageSize);

            var supportCodes = from sc in query
                               select new EquipmentCalibrationTypeModel
                               {
                                   Id = sc.Id,
                                   Name = sc.Name,
                                   Active = sc.Active
                               };

            var pagedList = new StaticPagedList<EquipmentCalibrationTypeModel>(supportCodes, page.Value, _adminAreaSettings.GridPageSize, totalCount);

            if (Request.IsAjaxRequest())
            {
                return PartialView(MVC.EquipmentCalibrationType.Views._Table, pagedList);
            }

            return View(pagedList);
        }

        public virtual ActionResult Create()
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var equipmentCalibrationType = new EquipmentCalibrationTypeModel
            {
                Active = true
            };

            return View(MVC.EquipmentCalibrationType.Views.CreateEdit, equipmentCalibrationType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(EquipmentCalibrationTypeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var equipmentCalibrationType = Mapper.Map<EquipmentCalibrationType>(model);
                    
                    equipmentCalibrationType.CreatedByUserId = _workContext.CurrentUser.Id;
                    equipmentCalibrationType.UpdatedByUserId = _workContext.CurrentUser.Id;
                    equipmentCalibrationType.DateCreatedOnUtc = DateTime.UtcNow;
                    equipmentCalibrationType.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._equipmentCalibrationTypeService.SaveEquipmentCalibrationType(equipmentCalibrationType);
                    SuccessNotification("Equipment Calibration Type has been created.");
                    return RedirectToAction(MVC.EquipmentCalibrationType.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.EquipmentCalibrationType.Views.CreateEdit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var entity = this._equipmentCalibrationTypeService.GetEquipmentCalibrationTypeById(id);

            var equipmentCalibrationType = Mapper.Map<EquipmentCalibrationTypeModel>(entity);

            return View(MVC.EquipmentCalibrationType.Views.CreateEdit, equipmentCalibrationType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(EquipmentCalibrationTypeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var equipmentCalibrationType = Mapper.Map<EquipmentCalibrationType>(model);
                    equipmentCalibrationType.UpdatedByUserId = _workContext.CurrentUser.Id;
                    equipmentCalibrationType.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._equipmentCalibrationTypeService.SaveEquipmentCalibrationType(equipmentCalibrationType);
                    SuccessNotification("Equipment Calibration Type has been saved.");
                    return RedirectToAction(MVC.EquipmentCalibrationType.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.EquipmentCalibrationType.Views.CreateEdit, model);
        }
        #endregion

    }
}