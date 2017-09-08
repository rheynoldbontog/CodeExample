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
    public class HighLevelBucketingController : BaseSSGController
    {
        #region Fields

        private readonly IHighLevelBucketingService _highLevelBucketingService;
        private readonly ILowLevelBucketingService _lowLevelBucketingService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly AdminAreaSettings _adminAreaSettings;

        #endregion

        #region Ctor

        public HighLevelBucketingController(
            IHighLevelBucketingService highLevelBucketingService,
            ILowLevelBucketingService lowLevelBucketingService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext,
            AdminAreaSettings adminAreaSettings)
        {
            this._highLevelBucketingService = highLevelBucketingService;
            this._lowLevelBucketingService = lowLevelBucketingService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._workContext = workContext;
            this._adminAreaSettings = adminAreaSettings;
        }

        #endregion

        #region Utilities

        protected HighLevelBucketingModel PrepareHighLevelBucketingModelForList(HighLevelBucketing bucket)
        {
            return new HighLevelBucketingModel()
            {
                Id = bucket.Id,
                HLBucket = bucket.HLBucket,
                Description = bucket.Description,
            };
        }

        protected LowLevelBucketingModel PrepareLowLevelBucketingModelForList(LowLevelBucketing bucket)
        {
            return new LowLevelBucketingModel()
            {
                Id = bucket.Id,
                LLBucket = bucket.LLBucket,
                Description = bucket.Description,
                HighLevelBucketingId = bucket.HighLevelBucketingId,
                HLBucket = bucket.HighLevelBucketing.HLBucket
            };
        }

        #endregion

        #region HighLevelBucketing

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageHighLevelBucketing))
                return AccessDeniedView();

            var listModel = new HighLevelBucketingListModel();

            // Get default list
            var bucket = _highLevelBucketingService.GetAllHighLevelBucketings(
                0, _adminAreaSettings.GridPageSize);

            // Move list to model
            listModel.HighLevelBucketings = new GridModel<HighLevelBucketingModel>
            {
                Data = bucket.Select(PrepareHighLevelBucketingModelForList),
                Total = bucket.TotalCount
            };

            return View(listModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command, HighLevelBucketingListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageHighLevelBucketing))
                return AccessDeniedView();

            var bucket = _highLevelBucketingService.GetAllHighLevelBucketings(
                command.Page - 1, _adminAreaSettings.GridPageSize);

            var gridModel = new GridModel<HighLevelBucketingModel>
            {
                Data = bucket.Select(PrepareHighLevelBucketingModelForList),
                Total = bucket.TotalCount
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult GetLowLevelBucketingList()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLowLevelBucketing))
                return AccessDeniedView();

            var listModel = new LowLevelBucketingListModel();

            // Get default list
            var lowLevelBuckets = _lowLevelBucketingService.GetAllLowLevelBucketings(0, 10);

            // Move list to model
            listModel.LowLevelBuckets = new GridModel<LowLevelBucketingModel>
            {
                Data = lowLevelBuckets.Select(PrepareLowLevelBucketingModelForList),
                Total = lowLevelBuckets.TotalCount
            };

            return View(listModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult GetLowLevelBucketingList(GridCommand command, LowLevelBucketingListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLowLevelBucketing))
                return AccessDeniedView();

            var lowLevelBuckets = _lowLevelBucketingService.GetAllLowLevelBucketings(
                command.Page - 1, 10);

            var gridModel = new GridModel<LowLevelBucketingModel>
            {
                Data = lowLevelBuckets.Select(PrepareLowLevelBucketingModelForList),
                Total = lowLevelBuckets.TotalCount
            };

            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageHighLevelBucketing))
                return AccessDeniedView();

            var model = new HighLevelBucketingModel();

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Create(HighLevelBucketingModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageHighLevelBucketing))
                return AccessDeniedView();

            // Make sure that the High Level Bucket is not existing
            if (!string.IsNullOrWhiteSpace(model.HLBucket))
            {
                var hlBucket = _highLevelBucketingService.GetHighLevelBucketingByBucket(model.HLBucket);
                if (hlBucket != null)
                    ModelState.AddModelError("", "High Level Bucketing is already existing");
            }

            if (ModelState.IsValid)
            {
                var bucket = new HighLevelBucketing();

                model.HLBucket = model.HLBucket.ToUpper();

                bucket.InjectFrom(model);

                #region Audits
                bucket.DateCreatedOnUtc = DateTime.UtcNow;
                bucket.DateUpdatedOnUtc = DateTime.UtcNow;
                bucket.CreatedByUserId = _workContext.CurrentUser.Id;
                bucket.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _highLevelBucketingService.InsertHighLevelBucketing(bucket);

                SuccessNotification(_localizationService.GetResource("FRACAS.HighLevelBucketing.Added"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = bucket.Id });

                // Save only
                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageHighLevelBucketing))
                return AccessDeniedView();

            var bucket = _highLevelBucketingService.GetHighLevelBucketingById(id);
            if (bucket == null)
                return RedirectToAction("List");

            var model = new HighLevelBucketingModel();

            // Get default list of Low Level Bucket
            var lowLevelBuckets = _lowLevelBucketingService.GetAllLowLevelBucketings(
                0, 10);

            // Move list to Low Level Bucket model
            model.LowLevelBuckets = new GridModel<LowLevelBucketingModel>
            {
                Data = lowLevelBuckets.Select(PrepareLowLevelBucketingModelForList),
                Total = lowLevelBuckets.TotalCount
            };

            model.InjectFrom(bucket);

            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-new", "saveNew")]
        [ParameterBasedOnFormName("save-continue", "saveContinue")]
        public ActionResult Edit(GridCommand command, HighLevelBucketingModel model, bool saveNew, bool saveContinue)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageHighLevelBucketing))
                return AccessDeniedView();

            // Make sure that the High Level Bucketing is not existing
            if (!string.IsNullOrWhiteSpace(model.HLBucket))
            {
                var hlBucket = _highLevelBucketingService.GetHighLevelBucketingByBucket(model.HLBucket);
                if (hlBucket != null && hlBucket.Id != model.Id)
                    ModelState.AddModelError("", "High Level Bucketing is already existing");
            }

            if (ModelState.IsValid)
            {
                var bucket = _highLevelBucketingService.GetHighLevelBucketingById(model.Id);

                if (bucket == null)
                    return RedirectToAction("List");

                model.HLBucket = model.HLBucket.ToUpper();

                bucket.InjectFrom(new IgnoreProperties("DateCreatedOnUtc", "CreatedByUserId"), model);

                #region Audits
                bucket.DateUpdatedOnUtc = DateTime.UtcNow;
                bucket.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _highLevelBucketingService.UpdateHighLevelBucketing(bucket);

                SuccessNotification(_localizationService.GetResource("FRACAS.HighLevelBucketing.Updated"));

                // SaveNew
                if (saveNew)
                    return RedirectToAction("Create");

                // SaveContinue
                if (saveContinue)
                    return RedirectToAction("Edit", new { id = bucket.Id });

                // Save only
                return RedirectToAction("List");
            }

            // Get default list of Low Level Bucket
            var lowLevelBuckets = _lowLevelBucketingService.GetAllLowLevelBucketings(
                command.Page - 1, 10);

            // Move list to Low Level Bucket model
            model.LowLevelBuckets = new GridModel<LowLevelBucketingModel>
            {
                Data = lowLevelBuckets.Select(PrepareLowLevelBucketingModelForList),
                Total = lowLevelBuckets.TotalCount
            };

            return View(model);
        }

        #endregion

    }
}
