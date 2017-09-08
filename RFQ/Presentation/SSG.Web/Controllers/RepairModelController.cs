using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Omu.ValueInjecter;
using SSG.Services.Fracas;
using SSG.Services.Localization;
using SSG.Services.Security;
using SSG.Core;
using SSG.Core.Domain.Common;
using SSG.Core.Domain.Fracas;
using SSG.Web.Models.Fracas;
using Telerik.Web.Mvc;
using SSG.Web.Framework.Controllers;
using SSG.Web.Infrastructure.Injecter;


namespace SSG.Web.Controllers
{
    public class RepairModelController : BaseSSGController
    {
        #region Fields

        private readonly IRepairModelService _repairModelService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly AdminAreaSettings _adminAreaSettings;

        #endregion

        #region Ctor

        public RepairModelController(
            IRepairModelService repairModelService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext,
            AdminAreaSettings adminAreaSettings)
        {
            this._repairModelService = repairModelService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._workContext = workContext;
            this._adminAreaSettings = adminAreaSettings;
        }
        
        #endregion

        #region Utilities

        protected RepairModelModel PrepareRepairModelModelForList(RepairModel repairModel)
        {
            return new RepairModelModel()
            {
                Id = repairModel.Id,
                Code = repairModel.Code,
                ResponsibleDivisionCode = repairModel.ResponsibleDivisionCode,
                PrimaryOrderCenter = repairModel.PrimaryOrderCenter,
                PrimaryRepairCenter = repairModel.PrimaryRepairCenter
            };
        }

        #endregion

        #region RepairModel

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRepairModel))
                return AccessDeniedView();

            var listModel = new RepairModelListModel();

            // Get default list
            var repairModels = _repairModelService.GetAllRepairModels(0, _adminAreaSettings.GridPageSize);

            // Move list to model
            listModel.RepairModels = new GridModel<RepairModelModel>
            {
                Data = repairModels.Select(PrepareRepairModelModelForList),
                Total = repairModels.TotalCount
            };

            return View(listModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command, RepairModelListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRepairModel))
                return AccessDeniedView();

            var repairModels = _repairModelService.GetAllRepairModels(command.Page - 1, _adminAreaSettings.GridPageSize);

            var gridModel = new GridModel<RepairModelModel>
            {
                Data = repairModels.Select(PrepareRepairModelModelForList),
                Total = repairModels.TotalCount
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRepairModel))
                return AccessDeniedView();

            var model = new RepairModelModel();

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Create(RepairModelModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRepairModel))
                return AccessDeniedView();

            // Make sure that the Repair Model is not existing
            if (!string.IsNullOrWhiteSpace(model.Code))
            {
                var repairModel = _repairModelService.GetRepairModelByModel(model.Code);
                if (repairModel != null)
                    ModelState.AddModelError("", "Repair Model is already existing");
            }

            if (ModelState.IsValid)
            {
                var repair = new RepairModel();

                model.Code = model.Code.ToUpper();
                model.ResponsibleDivisionCode = model.ResponsibleDivisionCode.ToUpper();
                model.PrimaryOrderCenter = model.PrimaryOrderCenter.ToUpper();
                model.PrimaryRepairCenter = model.PrimaryRepairCenter.ToUpper();

                repair.InjectFrom(model);

                #region Audits
                repair.DateCreatedOnUtc = DateTime.UtcNow;
                repair.DateUpdatedOnUtc = DateTime.UtcNow;
                repair.CreatedByUserId = _workContext.CurrentUser.Id;
                repair.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion


                _repairModelService.InsertRepairModel(repair);

                SuccessNotification(_localizationService.GetResource("FRACAS.RepairModel.Added"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = repair.Id });

                // Save only
                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRepairModel))
                return AccessDeniedView();

            var repairModel = _repairModelService.GetRepairModelById(id);
            if (repairModel == null)
                return RedirectToAction("List");

            var model = new RepairModelModel();

            model.InjectFrom(repairModel);

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Edit(RepairModelModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRepairModel))
                return AccessDeniedView();

            // Make sure that the Repair Model is not existing
            if (!string.IsNullOrWhiteSpace(model.Code))
            {
                var code = _repairModelService.GetRepairModelByModel(model.Code);
                if (code != null && code.Id != model.Id)
                    ModelState.AddModelError("", "Repair Model is already existing");
            }

            if (ModelState.IsValid)
            {
                var repairModel = _repairModelService.GetRepairModelById(model.Id);

                if (repairModel  == null)
                    return RedirectToAction("List");

                model.Code = model.Code.ToUpper();

                if (!string.IsNullOrWhiteSpace(model.ResponsibleDivisionCode))
                {
                    model.ResponsibleDivisionCode = model.ResponsibleDivisionCode.ToUpper();
                }
                else
                {
                    model.PrimaryOrderCenter = null;
                }

                if (!string.IsNullOrWhiteSpace(model.PrimaryOrderCenter))
                {
                    model.PrimaryOrderCenter = model.PrimaryOrderCenter.ToUpper();
                }
                else
                {
                    model.PrimaryOrderCenter = null;
                }

                if (!string.IsNullOrWhiteSpace(model.PrimaryRepairCenter))
                {
                    model.PrimaryRepairCenter = model.PrimaryRepairCenter.ToUpper();
                }
                else
                {
                    model.PrimaryOrderCenter = null;
                }

                repairModel.InjectFrom(new IgnoreProperties("DateCreatedOnUtc", "CreatedByUserId"), model);

                #region Audits
                repairModel.DateUpdatedOnUtc = DateTime.UtcNow;
                repairModel.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _repairModelService.UpdateRepairModel(repairModel);

                SuccessNotification(_localizationService.GetResource("FRACAS.RepairModel.Updated"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = repairModel.Id });

                // Save only
                return RedirectToAction("List");
            }

            return View(model);
        }

        #endregion

    }
}
