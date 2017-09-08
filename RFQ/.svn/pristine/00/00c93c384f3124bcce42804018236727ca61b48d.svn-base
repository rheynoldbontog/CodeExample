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
    public class DefectCodeController : BaseSSGController
    {
        #region Fields

        private readonly IDefectCodeService _defectCodeService;
        private readonly IDefectCategoryService _defectCategoryService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly AdminAreaSettings _adminAreaSettings;

        #endregion

        #region Ctor

        public DefectCodeController(
            IDefectCodeService defectCodeService,
            IDefectCategoryService defectCategoryService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext,
            AdminAreaSettings adminAreaSettings)
        {
            this._defectCodeService = defectCodeService;
            this._defectCategoryService = defectCategoryService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._workContext = workContext;
            this._adminAreaSettings = adminAreaSettings;
        }
        
        #endregion

        #region Utilities

        protected DefectCodeModel PrepareDefectCodeModelForList(DefectCode defectCode)
        {
            return new DefectCodeModel()
            {
                Id = defectCode.Id,
                Code = defectCode.Code,
                Description = defectCode.Description,
                DefectCategoryId = defectCode.DefectCategoryId,
                DefectCategory = defectCode.DefectCategory.Category
            };
        }

        #endregion

        #region DefectCode

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDefectCode))
                return AccessDeniedView();

            var listModel = new DefectCodeListModel();

            #region Dropdowns
            listModel.DefectCategoryList.Add(new SelectListItem() { Value = "", Text = "" });

            foreach (var category in _defectCategoryService.GetAllDefectCategories().OrderBy(x => x.Category))
                listModel.DefectCategoryList.Add(new SelectListItem() { Value = category.Id.ToString(), Text = category.Category });
            #endregion

            // Get default list
            var defectCode = _defectCodeService.GetAllDefectCodes(
                null, 0, 0, _adminAreaSettings.GridPageSize);

            // Move list to model
            listModel.DefectCodes = new GridModel<DefectCodeModel>
            {
                Data = defectCode.Select(PrepareDefectCodeModelForList),
                Total = defectCode.TotalCount
            };

            return View(listModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command, DefectCodeListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDefectCode))
                return AccessDeniedView();

            var defectCodes = _defectCodeService.GetAllDefectCodes(
                model.SearchCode, model.SearchCategoryId, 
                command.Page - 1, _adminAreaSettings.GridPageSize);

            var gridModel = new GridModel<DefectCodeModel>
            {
                Data = defectCodes.Select(PrepareDefectCodeModelForList),
                Total = defectCodes.TotalCount
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDefectCode))
                return AccessDeniedView();

            var model = new DefectCodeModel();

            #region Dropdowns
            model.DefectCategoryList.Add(new SelectListItem() { Value = "", Text = "" });

            foreach (var category in _defectCategoryService.GetAllDefectCategories().OrderBy(x => x.Category))
                model.DefectCategoryList.Add(new SelectListItem() { Value = category.Id.ToString(), Text = category.Category });
            #endregion

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Create(DefectCodeModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDefectCode))
                return AccessDeniedView();

            // Make sure that the Defect Code is not existing
            if (!string.IsNullOrWhiteSpace(model.Code))
            {
                var code = _defectCodeService.GetDefectCodeByCode(model.Code);
                if (code != null)
                    ModelState.AddModelError("", "Defect Code is already existing");
            }

            if (ModelState.IsValid)
            {
                var defectCode = new DefectCode();

                model.Code= model.Code.ToUpper();

                defectCode.InjectFrom(model);

                #region Audits
                defectCode.DateCreatedOnUtc = DateTime.UtcNow;
                defectCode.DateUpdatedOnUtc = DateTime.UtcNow;
                defectCode.CreatedByUserId = _workContext.CurrentUser.Id;
                defectCode.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion


                _defectCodeService.InsertDefectCode(defectCode);

                SuccessNotification(_localizationService.GetResource("FRACAS.DefectCode.Added"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = defectCode.Id });

                // Save only
                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDefectCode))
                return AccessDeniedView();

            var defectCode = _defectCodeService.GetDefectCodeById(id);
            if (defectCode == null)
                return RedirectToAction("List");

            var model = new DefectCodeModel();

            #region Dropdowns
            model.DefectCategoryList.Add(new SelectListItem() { Value = "", Text = "" });

            foreach (var category in _defectCategoryService.GetAllDefectCategories().OrderBy(x => x.Category))
                model.DefectCategoryList.Add(new SelectListItem() { Value = category.Id.ToString(), Text = category.Category });
            #endregion

            model.InjectFrom(defectCode);

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Edit(DefectCodeModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDefectCode))
                return AccessDeniedView();

            // Make sure that the Defect Code is not existing
            if (!string.IsNullOrWhiteSpace(model.Code))
            {
                var type = _defectCodeService.GetDefectCodeByCode(model.Code);
                if (type != null && type.Id != model.Id)
                    ModelState.AddModelError("", "Defect Code is already existing");
            }

            if (ModelState.IsValid)
            {
                var defectCode = _defectCodeService.GetDefectCodeById(model.Id);

                if (defectCode == null)
                    return RedirectToAction("List");

                model.Code= model.Code.ToUpper();
                
                defectCode.InjectFrom(new IgnoreProperties("DateCreatedOnUtc", "CreatedByUserId"), model);

                #region Audits
                defectCode.DateUpdatedOnUtc = DateTime.UtcNow;
                defectCode.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _defectCodeService.UpdateDefectCode(defectCode);

                SuccessNotification(_localizationService.GetResource("FRACAS.DefectCode.Updated"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = defectCode.Id });

                // Save only
                return RedirectToAction("List");
            }

            return View(model);
        }

        #endregion
    }
}
