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

namespace SSG.Web.Controllers
{
    public class LowLevelBucketingController : BaseSSGController
    {
        #region Fields

        private readonly ILowLevelBucketingService _lowLevelBucketingService;
        private readonly IHighLevelBucketingService _highLevelBucketingService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly AdminAreaSettings _adminAreaSettings;

        #endregion

        #region Ctor

        public LowLevelBucketingController(
            ILowLevelBucketingService lowLevelBucketingService,
            IHighLevelBucketingService highLevelBucketingService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext,
            AdminAreaSettings adminAreaSettings)
        {
            this._lowLevelBucketingService = lowLevelBucketingService;
            this._highLevelBucketingService = highLevelBucketingService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._workContext = workContext;
            this._adminAreaSettings = adminAreaSettings;
        }
        
        #endregion

        #region Utilities

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
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLowLevelBucketing))
                return AccessDeniedView();

            var listModel = new LowLevelBucketingListModel();

            #region Dropdowns
            listModel.HighLevelBucketingList.Add(new SelectListItem() { Value = "", Text = "" });

            foreach (var highLevelBucket in _highLevelBucketingService.GetAllHighLevelBucketings().OrderBy(x => x.HLBucket))
                listModel.HighLevelBucketingList.Add(new SelectListItem() { Value = highLevelBucket.Id.ToString(), Text = highLevelBucket.HLBucket });
            #endregion

            // Get default list
            var lowLevelBuckets = _lowLevelBucketingService.GetAllLowLevelBucketings(
                null, 0, 0, _adminAreaSettings.GridPageSize);

            // Move list to model
            listModel.LowLevelBuckets= new GridModel<LowLevelBucketingModel>
            {
                Data = lowLevelBuckets.Select(PrepareLowLevelBucketingModelForList),
                Total = lowLevelBuckets.TotalCount
            };

            return View(listModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command, LowLevelBucketingListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLowLevelBucketing))
                return AccessDeniedView();

            var lowLevelBuckets= _lowLevelBucketingService.GetAllLowLevelBucketings(
                model.SearchLowLevelBucketing, model.SearchHighLevelBucketingId,
                command.Page - 1, _adminAreaSettings.GridPageSize);

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
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLowLevelBucketing))
                return AccessDeniedView();

            var model = new LowLevelBucketingModel();

            #region Dropdowns
            model.HighLevelBucketingList.Add(new SelectListItem() { Value = "", Text = "" });

            foreach (var highLevelBucket in _highLevelBucketingService.GetAllHighLevelBucketings().OrderBy(x => x.HLBucket))
                model.HighLevelBucketingList.Add(new SelectListItem() { Value = highLevelBucket.Id.ToString(), Text = highLevelBucket.HLBucket });
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(LowLevelBucketingModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLowLevelBucketing))
                return AccessDeniedView();

            // Make sure that the Low Level Bucketing is not existing
            if (!string.IsNullOrWhiteSpace(model.LLBucket))
            {
                var lowLevelBucket= _lowLevelBucketingService.GetLowLevelBucketingByBucket(model.LLBucket);
                if (lowLevelBucket != null)
                    ModelState.AddModelError("", "Low Level Bucketing is already existing");
            }

            if (ModelState.IsValid)
            {
                var bucket = new LowLevelBucketing();

                model.LLBucket = model.LLBucket.ToUpper();

                bucket.InjectFrom(model);

                #region Audits
                bucket.DateCreatedOnUtc = DateTime.UtcNow;
                bucket.DateUpdatedOnUtc = DateTime.UtcNow;
                bucket.CreatedByUserId = _workContext.CurrentUser.Id;
                bucket.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _lowLevelBucketingService.InsertLowLevelBucketing(bucket);

                SuccessNotification(_localizationService.GetResource("FRACAS.LowLevelBucketing.Added"));

                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLowLevelBucketing))
                return AccessDeniedView();

            var lowLevelBucket = _lowLevelBucketingService.GetLowLevelBucketingById(id);
            if (lowLevelBucket == null)
                return RedirectToAction("List");

            var model = new LowLevelBucketingModel();

            #region Dropdowns
            model.HighLevelBucketingList.Add(new SelectListItem() { Value = "", Text = "" });

            foreach (var highLevelBucket in _highLevelBucketingService.GetAllHighLevelBucketings().OrderBy(x => x.HLBucket))
                model.HighLevelBucketingList.Add(new SelectListItem() { Value = highLevelBucket.Id.ToString(), Text = highLevelBucket.HLBucket });
            #endregion

            model.InjectFrom(lowLevelBucket);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(LowLevelBucketingModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLowLevelBucketing))
                return AccessDeniedView();

            // Make sure that the Low Level Bucketing is not existing
            if (!string.IsNullOrWhiteSpace(model.LLBucket))
            {
                var lowLevelBucket = _lowLevelBucketingService.GetLowLevelBucketingByBucket(model.LLBucket);
                if (lowLevelBucket != null && lowLevelBucket.Id != model.Id)
                    ModelState.AddModelError("", "Low Level Bucketing is already existing");
            }

            if (ModelState.IsValid)
            {
                var bucket = _lowLevelBucketingService.GetLowLevelBucketingById(model.Id);

                if (bucket == null)
                    return RedirectToAction("List");

                model.LLBucket = model.LLBucket.ToUpper();

                bucket.InjectFrom(new IgnoreProperties("DateCreatedOnUtc", "CreatedByUserId"), model);

                #region Audits
                bucket.DateUpdatedOnUtc = DateTime.UtcNow;
                bucket.UpdatedByUserId = _workContext.CurrentUser.Id;
                #endregion

                _lowLevelBucketingService.UpdateLowLevelBucketing(bucket);

                SuccessNotification(_localizationService.GetResource("FRACAS.LowLevelBucketing.Updated"));

                return RedirectToAction("List");
            }

            return View(model);
        }

        #endregion
    }
}
