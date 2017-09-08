using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSG.Services.RFQ;
using SSG.Core.Domain.Common;
using SSG.Core;
using SSG.Services.Security;
using System.Web.Mvc;
using SSG.Web.Models.RFQ;
using PagedList;
using AutoMapper;
using SSG.Core.Domain.RFQ;

namespace SSG.Web.Controllers
{
    public partial class RFQStatusController : BaseSSGController
    {
        #region Fields

        private readonly IRFQStatusService _rFQStatusService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;

        #endregion

        #region ctor

        public RFQStatusController(IRFQStatusService rFQStatusService,
            AdminAreaSettings adminAreaSettings,
            IWorkContext workContext,
            IPermissionService permissionService
            )
        {
            this._rFQStatusService = rFQStatusService;
            this._adminAreaSettings = adminAreaSettings;
            this._workContext = workContext;
            this._permissionService = permissionService;
        }

        #endregion

        #region Methods

        public virtual ActionResult Index(int? page)
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            page = page ?? 1;

            var query = this._rFQStatusService.GetAllRFQStatusQueryable();

            int totalCount = query.Count();

            query = query.OrderBy(s => s.DateUpdatedOnUtc).Skip((page.Value - 1) * _adminAreaSettings.GridPageSize).Take(_adminAreaSettings.GridPageSize);

            var supportCodes = from sc in query
                               select new RFQStatusModel
                               {
                                   Id = sc.Id,
                                   Name = sc.Name,
                                   Active = sc.Active
                               };

            var pagedList = new StaticPagedList<RFQStatusModel>(supportCodes, page.Value, _adminAreaSettings.GridPageSize, totalCount);

            if (Request.IsAjaxRequest())
            {
                return PartialView(MVC.RFQStatus.Views._Table, pagedList);
            }

            return View(pagedList);
        }

        public virtual ActionResult Create()
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var rfqStatus = new RFQStatusModel
            {
                Active = true
            };

            return View(MVC.RFQStatus.Views.CreateEdit, rfqStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(RFQStatusModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rfqStatus = Mapper.Map<RFQStatus>(model);

                    rfqStatus.CreatedByUserId = _workContext.CurrentUser.Id;
                    rfqStatus.UpdatedByUserId = _workContext.CurrentUser.Id;
                    rfqStatus.DateCreatedOnUtc = DateTime.UtcNow;
                    rfqStatus.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._rFQStatusService.SaveRFQStatus(rfqStatus);
                    SuccessNotification("RFQ Status has been created.");
                    return RedirectToAction(MVC.RFQStatus.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.RFQStatus.Views.CreateEdit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var entity = this._rFQStatusService.GetRFQStatusById(id);

            var rfqStatus = Mapper.Map<RFQStatusModel>(entity);

            return View(MVC.RFQStatus.Views.CreateEdit, rfqStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(RFQStatusModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rfqStatus = Mapper.Map<RFQStatus>(model);
                    rfqStatus.UpdatedByUserId = _workContext.CurrentUser.Id;
                    rfqStatus.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._rFQStatusService.SaveRFQStatus(rfqStatus);
                    SuccessNotification("RFQ Status has been saved.");
                    return RedirectToAction(MVC.RFQStatus.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.RFQStatus.Views.CreateEdit, model);
        }

        #endregion
    }
}