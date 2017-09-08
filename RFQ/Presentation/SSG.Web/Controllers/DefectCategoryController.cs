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
    public class DefectCategoryController : BaseSSGController
    {
        #region Fields

        private readonly IDefectCategoryService _defectCategoryService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly AdminAreaSettings _adminAreaSettings;

        #endregion

        #region Ctor

        public DefectCategoryController(
            IDefectCategoryService defectCategoryService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext,
            AdminAreaSettings adminAreaSettings)
        {
            this._defectCategoryService = defectCategoryService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._workContext = workContext;
            this._adminAreaSettings = adminAreaSettings;
        }
        
        #endregion

        #region Utilities

        protected DefectCategoryModel PrepareDefectCategoryModelForList(DefectCategory defectCategory)
        {
            return new DefectCategoryModel()
            {
                Id = defectCategory.Id,
                Category = defectCategory.Category,
                Description = defectCategory.Description,
            };
        }

        #endregion

        #region DefectCategory

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDefectCategory))
                return AccessDeniedView();

            var listModel = new DefectCategoryListModel();

            // Get default list
            var defectCategory = _defectCategoryService.GetAllDefectCategories(
                null, 0, _adminAreaSettings.GridPageSize);

            // Move list to model
            listModel.DefectCategories = new GridModel<DefectCategoryModel>
            {
                Data = defectCategory.Select(PrepareDefectCategoryModelForList),
                Total = defectCategory.TotalCount
            };

            return View(listModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command, DefectCategoryListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDefectCategory))
                return AccessDeniedView();

            var defectCategory = _defectCategoryService.GetAllDefectCategories(
                model.SearchCategory,
                command.Page - 1, 
                _adminAreaSettings.GridPageSize);

            var gridModel = new GridModel<DefectCategoryModel>
            {
                Data = defectCategory.Select(PrepareDefectCategoryModelForList),
                Total = defectCategory.TotalCount
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDefectCategory))
                return AccessDeniedView();

            var model = new DefectCategoryModel();

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Create(DefectCategoryModel model, bool saveContinue, bool saveNew)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDefectCategory))
                return AccessDeniedView();

            // Make sure that the Defect Category is not existing
            if (!string.IsNullOrWhiteSpace(model.Category))
            {
                var category = _defectCategoryService.GetDefectCategoryByCategory(model.Category);
                if (category != null)
                    ModelState.AddModelError("", "Defect Category is already existing");
            }

            if (ModelState.IsValid)
            {
                var defectCategory = new DefectCategory();

                model.Category = model.Category.ToUpper();

                defectCategory.InjectFrom(model);

                #region Audits
                defectCategory.DateCreatedOnUtc = DateTime.UtcNow;
                defectCategory.DateUpdatedOnUtc = DateTime.UtcNow;
                defectCategory.CreatedByUserId = _workContext.CurrentUser.Id;
                defectCategory.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion


                _defectCategoryService.InsertDefectCategory(defectCategory);

                SuccessNotification(_localizationService.GetResource("FRACAS.DefectCategory.Added"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = defectCategory.Id });

                // Save Only

                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDefectCategory))
                return AccessDeniedView();

            var defectCategory = _defectCategoryService.GetDefectCategoryById(id);
            if (defectCategory == null)
                return RedirectToAction("List");

            var model = new DefectCategoryModel();

            model.InjectFrom(defectCategory);

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Edit(DefectCategoryModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDefectCategory))
                return AccessDeniedView();

            // Make sure that the Defect Category is not existing
            if (!string.IsNullOrWhiteSpace(model.Category))
            {
                var category = _defectCategoryService.GetDefectCategoryByCategory(model.Category);
                if (category != null && category.Id != model.Id)
                    ModelState.AddModelError("", "Defect Category is already existing");
            }

            if (ModelState.IsValid)
            {
                var defectCategory = _defectCategoryService.GetDefectCategoryById(model.Id);

                if (defectCategory == null)
                    return RedirectToAction("List");

                model.Category = model.Category.ToUpper();

                defectCategory.InjectFrom(new IgnoreProperties("DateCreatedOnUtc", "CreatedByUserId"), model);

                #region Audits
                defectCategory.DateUpdatedOnUtc = DateTime.UtcNow;
                defectCategory.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _defectCategoryService.UpdateDefectCategory(defectCategory);

                SuccessNotification(_localizationService.GetResource("FRACAS.DefectCategory.Updated"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = defectCategory.Id });

                // Save only
                return RedirectToAction("List");
            }

            return View(model);
        }

        #endregion
    }
}
