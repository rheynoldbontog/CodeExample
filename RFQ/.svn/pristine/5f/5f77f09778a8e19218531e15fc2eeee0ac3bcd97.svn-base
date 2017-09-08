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
    public class RootCauseCategoryController : BaseSSGController
    {
        #region Fields

        private readonly IRootCauseCategoryService _rootCauseCategoryService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly AdminAreaSettings _adminAreaSettings;

        #endregion

        #region Ctor

        public RootCauseCategoryController(
            IRootCauseCategoryService rootCauseCategoryService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext,
            AdminAreaSettings adminAreaSettings)
        {
            this._rootCauseCategoryService = rootCauseCategoryService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._workContext = workContext;
            this._adminAreaSettings = adminAreaSettings;
        }

        #endregion

        #region Utilities

        protected RootCauseCategoryModel PrepareRootCauseCategoryModelForList(RootCauseCategory category)
        {
            return new RootCauseCategoryModel()
            {
                Id = category.Id,
                Category = category.Category,
                Description = category.Description,
            };
        }

        #endregion

        #region RootCauseCategory

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRootCauseCategory))
                return AccessDeniedView();

            var listModel = new RootCauseCategoryListModel();

            // Get default list
            var category = _rootCauseCategoryService.GetAllRootCauseCategories(0, _adminAreaSettings.GridPageSize);

            // Move list to model
            listModel.RootCauseCategories = new GridModel<RootCauseCategoryModel>
            {
                Data = category.Select(PrepareRootCauseCategoryModelForList),
                Total = category.TotalCount
            };

            return View(listModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command, RootCauseCategoryModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRootCauseCategory))
                return AccessDeniedView();

            var categories = _rootCauseCategoryService.GetAllRootCauseCategories(command.Page - 1, _adminAreaSettings.GridPageSize);

            var gridModel = new GridModel<RootCauseCategoryModel>
            {
                Data = categories.Select(PrepareRootCauseCategoryModelForList),
                Total = categories.TotalCount
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRootCauseCategory))
                return AccessDeniedView();

            var model = new RootCauseCategoryModel();

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Create(RootCauseCategoryModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRootCauseCategory))
                return AccessDeniedView();

            // Make sure that the Root Cause Category is not existing
            if (!string.IsNullOrWhiteSpace(model.Category))
            {
                var category = _rootCauseCategoryService.GetRootCauseCategoryByCategory(model.Category);
                if (category != null)
                    ModelState.AddModelError("", "Root Cause Category is already existing");
            }

            if (ModelState.IsValid)
            {
                var rootCauseCategory = new RootCauseCategory();

                model.Category = model.Category.ToUpper();

                rootCauseCategory.InjectFrom(model);

                #region Audits
                rootCauseCategory.DateCreatedOnUtc = DateTime.UtcNow;
                rootCauseCategory.DateUpdatedOnUtc = DateTime.UtcNow;
                rootCauseCategory.CreatedByUserId = _workContext.CurrentUser.Id;
                rootCauseCategory.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _rootCauseCategoryService.InsertRootCauseCategory(rootCauseCategory);

                SuccessNotification(_localizationService.GetResource("FRACAS.RootCauseCategory.Added"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = rootCauseCategory.Id });

                // Save only
                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRootCauseCategory))
                return AccessDeniedView();

            var category = _rootCauseCategoryService.GetRootCauseCategoryById(id);
            if (category == null)
                return RedirectToAction("List");

            var model = new RootCauseCategoryModel();

            model.InjectFrom(category);

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Edit(RootCauseCategoryModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageRootCauseCategory))
                return AccessDeniedView();

            // Make sure that the Root Cause Category is not existing
            if (!string.IsNullOrWhiteSpace(model.Category))
            {
                var category = _rootCauseCategoryService.GetRootCauseCategoryByCategory(model.Category);
                if (category != null && category.Id != model.Id)
                    ModelState.AddModelError("", "Root Cause Category is already existing");
            }

            if (ModelState.IsValid)
            {
                var rootCauseCategory = _rootCauseCategoryService.GetRootCauseCategoryById(model.Id);

                if (rootCauseCategory == null)
                    return RedirectToAction("List");

                model.Category = model.Category.ToUpper();

                rootCauseCategory.InjectFrom(new IgnoreProperties("DateCreatedOnUtc", "CreatedByUserId"), model);

                #region Audits
                rootCauseCategory.DateUpdatedOnUtc = DateTime.UtcNow;
                rootCauseCategory.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _rootCauseCategoryService.UpdateRootCauseCategory(rootCauseCategory);

                SuccessNotification(_localizationService.GetResource("FRACAS.RootCauseCategory.Updated"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = rootCauseCategory.Id });

                // Save only
                return RedirectToAction("List");
            }

            return View(model);
        }

        #endregion

    }
}
