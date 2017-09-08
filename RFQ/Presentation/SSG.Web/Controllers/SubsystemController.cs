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
    public class SubsystemController : BaseSSGController
    {
        #region Fields

        private readonly ISubsystemService _subsystemService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly AdminAreaSettings _adminAreaSettings;

        #endregion

        #region Ctor

        public SubsystemController(
            ISubsystemService subsystemService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext,
            AdminAreaSettings adminAreaSettings)
        {
            this._subsystemService = subsystemService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._workContext = workContext;
            this._adminAreaSettings = adminAreaSettings;
        }

        #endregion

        #region Utilities

        protected SubsystemModel PrepareSubsystemModelForList(SubSystem system)
        {
            return new SubsystemModel()
            {
                Id = system.Id,
                Name = system.Name,
                Description = system.Description
            };
        }

        #endregion

        #region Subsystem

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSubsystem))
                return AccessDeniedView();

            var listModel = new SubsystemListModel();

            // Get default list
            var subsystems = _subsystemService.GetAllSubsystems(
                null, 0, _adminAreaSettings.GridPageSize);

            // Move list to model
            listModel.Subsystems = new GridModel<SubsystemModel>
            {
                Data = subsystems.Select(PrepareSubsystemModelForList),
                Total = subsystems.TotalCount
            };

            return View(listModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command, SubsystemListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSubsystem))
                return AccessDeniedView();

            var subsystems = _subsystemService.GetAllSubsystems(model.SearchName, command.Page - 1, _adminAreaSettings.GridPageSize);

            var gridModel = new GridModel<SubsystemModel>
            {
                Data = subsystems.Select(PrepareSubsystemModelForList),
                Total = subsystems.TotalCount
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSubsystem))
                return AccessDeniedView();

            var model = new SubsystemModel();

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Create(SubsystemModel model, bool saveContinue, bool saveNew)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSubsystem))
                return AccessDeniedView();

            // Make sure Subsystem is not existing
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                var name = _subsystemService.GetSubsystemByName(model.Name);
                if (name != null)
                    ModelState.AddModelError("", "Subsystem is already existing.");
            }

            if (ModelState.IsValid)
            {
                var subsystem = new SubSystem();

                model.Name = model.Name.ToUpper();
                model.Description = model.Description.ToUpper();

                subsystem.InjectFrom(model);

                #region Audit
                subsystem.DateCreatedOnUtc = DateTime.UtcNow;
                subsystem.DateUpdatedOnUtc = DateTime.UtcNow;
                subsystem.CreatedByUserId = _workContext.CurrentUser.Id;
                subsystem.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _subsystemService.InsertSubSystem(subsystem);

                SuccessNotification(_localizationService.GetResource("FRACAS.Subsystem.Added"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = subsystem.Id });

                // Save Only
                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSubsystem))
                return AccessDeniedView();

            var name = _subsystemService.GetSubSystemById(id);
            if (name == null)
                return RedirectToAction("List");

            var model = new SubsystemModel();

            model.InjectFrom(name);

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Edit(SubsystemModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSubsystem))
                return AccessDeniedView();

            // Make sure Subsystem is not existing
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                var name = _subsystemService.GetSubsystemByName(model.Name);
                if (name != null && name.Id != model.Id)
                    ModelState.AddModelError("", "Subsystem is already existing.");
            }

            if (ModelState.IsValid)
            {
                var subsystem = _subsystemService.GetSubSystemById(model.Id);

                if (subsystem == null)
                    return RedirectToAction("List");

                model.Name = model.Name.ToUpper();
                model.Description = model.Description.ToUpper();

                subsystem.InjectFrom(new IgnoreProperties("DateCreatedOnUtc", "CreatedByUserId"), model);

                #region Audits
                subsystem.DateUpdatedOnUtc = DateTime.UtcNow;
                subsystem.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _subsystemService.UpdateSubSystem(subsystem);

                SuccessNotification(_localizationService.GetResource("FRACAS.SystemType.Added"));

                if (saveNew)
                    return RedirectToAction("Create");

                if (saveContinue)
                    return RedirectToAction("Edit", new { id = subsystem.Id });

                return RedirectToAction("List");
            }

            return View(model);
        }

        #endregion
    }
}
