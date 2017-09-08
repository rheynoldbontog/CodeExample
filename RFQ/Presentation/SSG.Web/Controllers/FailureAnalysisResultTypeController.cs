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
using Omu.ValueInjecter;
using SSG.Web.Infrastructure.Injecter;
using SSG.Web.Framework.Controllers;

namespace SSG.Web.Controllers
{
    public class FailureAnalysisResultTypeController : BaseSSGController
    {
        #region Fields

        private readonly IFailureAnalysisResultTypeService _failureAnalysisResultTypeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly AdminAreaSettings _adminAreaSettings;

        #endregion

        #region Ctor

        public FailureAnalysisResultTypeController(
            IFailureAnalysisResultTypeService failureAnalysisResultTypeService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext,
            AdminAreaSettings adminAreaSettings)
        {
            this._failureAnalysisResultTypeService = failureAnalysisResultTypeService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._workContext = workContext;
            this._adminAreaSettings = adminAreaSettings;
        }
        
        #endregion

        #region Utilities

        protected FailureAnalysisResultTypeModel PrepareFailureAnalysisTypeModelForList(FailureAnalysisResultType type)
        {
            return new FailureAnalysisResultTypeModel()
            {
               Id = type.Id,
               AnalysisType = type.AnalysisType,
               Description = type.Description
            };
        }

        #endregion

        #region FailureAnalysisResultType

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageFailureAnalysisResultType))
                return AccessDeniedView();

            var listModel = new FailureAnalysisResultTypeListModel();

            #region Dropdowns
            listModel.FailureAnalysisResultTypeList.Add(new SelectListItem() { Value = "", Text = "" });

            foreach (var type in _failureAnalysisResultTypeService.GetAllFailureAnalysisResultTypes().OrderBy(x => x.AnalysisType))
                listModel.FailureAnalysisResultTypeList.Add(new SelectListItem() { Value = type.AnalysisType, Text = type.AnalysisType});
            #endregion

            // Get default list
            var failureAnalysisType= _failureAnalysisResultTypeService.GetAllFailureAnalysisResultTypes(
                null, 0, _adminAreaSettings.GridPageSize);

            // Move list to model
            listModel.FailureAnalysisResultTypes = new GridModel<FailureAnalysisResultTypeModel>
            {
                Data = failureAnalysisType.Select(PrepareFailureAnalysisTypeModelForList),
                Total = failureAnalysisType.TotalCount
            };

            return View(listModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command, FailureAnalysisResultTypeListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageFailureAnalysisResultType))
                return AccessDeniedView();

            var failureAnalysisType = _failureAnalysisResultTypeService.GetAllFailureAnalysisResultTypes(
                model.SearchFailureAnalysisResultType,
                command.Page - 1, 
                _adminAreaSettings.GridPageSize);

            var gridModel = new GridModel<FailureAnalysisResultTypeModel>
            {
                Data = failureAnalysisType.Select(PrepareFailureAnalysisTypeModelForList),
                Total = failureAnalysisType.TotalCount
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageFailureAnalysisResultType))
                return AccessDeniedView();

            var model = new FailureAnalysisResultTypeModel();

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Create(FailureAnalysisResultTypeModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageFailureAnalysisResultType))
                return AccessDeniedView();

            // Make sure that the Analysis Type is not existing
            if (!string.IsNullOrWhiteSpace(model.AnalysisType))
            {
                var type = _failureAnalysisResultTypeService.GetAnalysisTypeByType(model.AnalysisType);
                if (type != null)
                    ModelState.AddModelError("", "Failure Analysis Type is already existing");
            }

            if (ModelState.IsValid)
            {
                var failureAnalysisType = new FailureAnalysisResultType();

                model.AnalysisType = model.AnalysisType.ToUpper();

                failureAnalysisType.InjectFrom(model);

                #region Audits
                failureAnalysisType.DateCreatedOnUtc = DateTime.UtcNow;
                failureAnalysisType.DateUpdatedOnUtc = DateTime.UtcNow;
                failureAnalysisType.CreatedByUserId = _workContext.CurrentUser.Id;
                failureAnalysisType.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion


                _failureAnalysisResultTypeService.InsertFailureAnalysisResultType(failureAnalysisType);

                SuccessNotification(_localizationService.GetResource("FRACAS.FailureAnalysisResultType.Added"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = failureAnalysisType.Id });

                // Save only
                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageFailureAnalysisResultType))
                return AccessDeniedView();

            var failureAnalysisType = _failureAnalysisResultTypeService.GetFailureAnalysisResultTypeById(id);
            if (failureAnalysisType == null)
                return RedirectToAction("List");

            var model = new FailureAnalysisResultTypeModel();

            model.InjectFrom(failureAnalysisType);

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Edit(FailureAnalysisResultTypeModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageFailureAnalysisResultType))
                return AccessDeniedView();

            // Make sure that the Analysis Type is not existing
            if (!string.IsNullOrWhiteSpace(model.AnalysisType))
            {
                var type = _failureAnalysisResultTypeService.GetAnalysisTypeByType(model.AnalysisType);
                if (type != null && type.Id != model.Id)
                    ModelState.AddModelError("", "Failure Analysis Type is already existing");
            }

            if (ModelState.IsValid)
            {
                var failureAnalysisType = _failureAnalysisResultTypeService.GetFailureAnalysisResultTypeById(model.Id);

                if (failureAnalysisType == null)
                    return RedirectToAction("List");

                model.AnalysisType = model.AnalysisType.ToUpper();

                failureAnalysisType.InjectFrom(new IgnoreProperties("DateCreatedOnUtc", "CreatedByUserId"), model);

                #region Audits
                failureAnalysisType.DateUpdatedOnUtc = DateTime.UtcNow;
                failureAnalysisType.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _failureAnalysisResultTypeService.UpdateFailureAnalysisResultType(failureAnalysisType);

                SuccessNotification(_localizationService.GetResource("FRACAS.FailureAnalaysisResultType.Update"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = failureAnalysisType.Id });

                // Save only
                return RedirectToAction("List");
            }

            return View(model);
        }

        #endregion
    }
}
