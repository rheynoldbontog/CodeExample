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
    public class NoChargeCodeController : BaseSSGController
    {
         #region Fields

        private readonly INoChargeCodeService _noChargeCodeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly AdminAreaSettings _adminAreaSettings;

        #endregion

        #region Ctor

        public NoChargeCodeController(
            INoChargeCodeService noChargeCodeService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext,
            AdminAreaSettings adminAreaSettings)
        {
            this._noChargeCodeService = noChargeCodeService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._workContext = workContext;
            this._adminAreaSettings = adminAreaSettings;
        }

        #endregion

        #region Utilities

        protected NoChargeCodeModel PrepareNoChargeCodeModelForList(NoChargeCode noChargeCode)
        {
            return new NoChargeCodeModel()
            {
                Id = noChargeCode.Id,
                Code = noChargeCode.Code,
                ShortDescription = noChargeCode.ShortDescription,
                Description = noChargeCode.Description,
                Comment = noChargeCode.Comment
            };
        }

        #endregion

        #region NoChargeCode

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNoChargeCode))
                return AccessDeniedView();

            var listModel = new NoChargeCodeListModel();

            // Get default list
            var codes = _noChargeCodeService.GetAllNoChargeCodes(0, 5);

            // Move list to model
            listModel.NoChargeCodes = new GridModel<NoChargeCodeModel>
            {
                Data = codes.Select(PrepareNoChargeCodeModelForList),
                Total = codes.TotalCount
            };

            return View(listModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command, NoChargeCodeListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNoChargeCode))
                return AccessDeniedView();

            var codes = _noChargeCodeService.GetAllNoChargeCodes(command.Page - 1, 5);

            var gridModel = new GridModel<NoChargeCodeModel>
            {
                Data = codes.Select(PrepareNoChargeCodeModelForList),
                Total = codes.TotalCount
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNoChargeCode))
                return AccessDeniedView();

            var model = new NoChargeCodeModel();

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Create(NoChargeCodeModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNoChargeCode))
                return AccessDeniedView();

            // Make sure that the No Charge Code is not existing
            if (!string.IsNullOrWhiteSpace(model.Code))
            {
                var code = _noChargeCodeService.GetNoChargeCodeByCode(model.Code);
                if (code != null)
                    ModelState.AddModelError("", "No Charge Code is already existing");
            }

            if (ModelState.IsValid)
            {
                var noChargeCode = new NoChargeCode();

                model.Code = model.Code.ToUpper();

                noChargeCode.InjectFrom(model);

                #region Audits
                noChargeCode.DateCreatedOnUtc = DateTime.UtcNow;
                noChargeCode.DateUpdatedOnUtc = DateTime.UtcNow;
                noChargeCode.CreatedByUserId = _workContext.CurrentUser.Id;
                noChargeCode.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _noChargeCodeService.InsertNoChargeCode(noChargeCode);

                SuccessNotification(_localizationService.GetResource("FRACAS.NoChargeCode.Added"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = noChargeCode.Id });

                // Save only
                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNoChargeCode))
                return AccessDeniedView();

            var codes = _noChargeCodeService.GetNoChargeCodeById(id);
            if (codes == null)
                return RedirectToAction("List");

            var model = new NoChargeCodeModel();

            model.InjectFrom(codes);

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Edit(NoChargeCodeModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNoChargeCode))
                return AccessDeniedView();

            // Make sure that the No Charge Code is not existing
            if (!string.IsNullOrWhiteSpace(model.Code))
            {
                var code = _noChargeCodeService.GetNoChargeCodeByCode(model.Code);
                if (code != null && code.Id != model.Id)
                    ModelState.AddModelError("", "No Charge Code is already existing");
            }

            if (ModelState.IsValid)
            {
                var noChargeCode = _noChargeCodeService.GetNoChargeCodeById(model.Id);

                if (noChargeCode == null)
                    return RedirectToAction("List");

                model.Code = model.Code.ToUpper();

                noChargeCode.InjectFrom(new IgnoreProperties("DateCreatedOnUtc", "CreatedByUserId"), model);

                #region Audits
                noChargeCode.DateUpdatedOnUtc = DateTime.UtcNow;
                noChargeCode.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _noChargeCodeService.UpdateNoChargeCode(noChargeCode);

                SuccessNotification(_localizationService.GetResource("FRACAS.NoChargeCode.Updated"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = noChargeCode.Id });

                // Save only
                return RedirectToAction("List");
            }

            return View(model);
        }

        #endregion

    }
}
