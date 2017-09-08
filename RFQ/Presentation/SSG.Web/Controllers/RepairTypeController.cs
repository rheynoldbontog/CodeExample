using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSG.Services.Fracas;
using SSG.Services.Localization;
using SSG.Services.Security;
using SSG.Core;
using SSG.Core.Domain.Common;
using SSG.Web.Models.Fracas;
using SSG.Core.Domain.Fracas;
using Telerik.Web.Mvc;
using SSG.Web.Framework.Controllers;
using Omu.ValueInjecter;
using SSG.Web.Infrastructure.Injecter;

namespace SSG.Web.Controllers
{
    public class RepairTypeController : BaseSSGController
    {
        #region Fields

        private readonly IRepairTypeService _repairTypeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly AdminAreaSettings _adminAreaSettings;

        #endregion

        #region Ctor

        public RepairTypeController(
            IRepairTypeService repairTypeService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext,
            AdminAreaSettings adminAreaSettings)
        {
            this._repairTypeService = repairTypeService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._workContext = workContext;
            this._adminAreaSettings = adminAreaSettings;
        }
        
        #endregion

        #region Utilities

        protected RepairTypeModel PrepareDefectCodeModelForList(RepairType repairType)
        {
            return new RepairTypeModel()
            {
                Id = repairType.Id,
                Type = repairType.Type,
                Description = repairType.Description,
            };
        }

        #endregion

        #region RepairType

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRepairType))
                return AccessDeniedView();

            var listModel = new RepairTypeListModel();

            // Get default list
            var repairType = _repairTypeService.GetAllRepairTypes(0, _adminAreaSettings.GridPageSize);

            // Move list to model
            listModel.RepairTypes = new GridModel<RepairTypeModel>
            {
                Data = repairType.Select(PrepareDefectCodeModelForList),
                Total = repairType.TotalCount
            };

            return View(listModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command, RepairTypeModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRepairType))
                return AccessDeniedView();

            var repairTypes = _repairTypeService.GetAllRepairTypes(command.Page - 1, _adminAreaSettings.GridPageSize);

            var gridModel = new GridModel<RepairTypeModel>
            {
                Data = repairTypes.Select(PrepareDefectCodeModelForList),
                Total = repairTypes.TotalCount
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRepairType))
                return AccessDeniedView();

            var model = new RepairTypeModel();

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Create(RepairTypeModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRepairType))
                return AccessDeniedView();

            // Make sure that the Repair Type is not existing
            if (!string.IsNullOrWhiteSpace(model.Type))
            {
                var type = _repairTypeService.GetRepairTypeByType(model.Type);
                if (type != null)
                    ModelState.AddModelError("", "Repair Type is already existing");
            }

            if (ModelState.IsValid)
            {
                var repairType = new RepairType();

                model.Type = model.Type.ToUpper();

                repairType.InjectFrom(model);

                #region Audits
                repairType.DateCreatedOnUtc = DateTime.UtcNow;
                repairType.DateUpdatedOnUtc = DateTime.UtcNow;
                repairType.CreatedByUserId = _workContext.CurrentUser.Id;
                repairType.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion


                _repairTypeService.InsertRepairType(repairType);

                SuccessNotification(_localizationService.GetResource("FRACAS.RepairType.Added"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = repairType.Id });

                // Save only
                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRepairType))
                return AccessDeniedView();

            var type = _repairTypeService.GetRepairTypeById(id);
            if (type == null)
                return RedirectToAction("List");

            var model = new RepairTypeModel();

            model.InjectFrom(type);

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Edit(RepairTypeModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRepairType))
                return AccessDeniedView();

            // Make sure that the Repair Type is not existing
            if (!string.IsNullOrWhiteSpace(model.Type))
            {
                var type = _repairTypeService.GetRepairTypeByType(model.Type);
                if (type != null && type.Id != model.Id)
                    ModelState.AddModelError("", "Repair Type is already existing");
            }

            if (ModelState.IsValid)
            {
                var repairType = _repairTypeService.GetRepairTypeById(model.Id);

                if (repairType == null)
                    return RedirectToAction("List");

                model.Type = model.Type.ToUpper();

                repairType.InjectFrom(new IgnoreProperties("DateCreatedOnUtc", "CreatedByUserId"), model);

                #region Audits
                repairType.DateUpdatedOnUtc = DateTime.UtcNow;
                repairType.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _repairTypeService.UpdateRepairType(repairType);

                SuccessNotification(_localizationService.GetResource("FRACAS.RepairType.Updated"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = repairType.Id });

                // Save only
                return RedirectToAction("List");
            }

            return View(model);
        }

        #endregion

    }
}
