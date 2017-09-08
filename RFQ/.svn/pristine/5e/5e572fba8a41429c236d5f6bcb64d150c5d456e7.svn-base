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
    public partial class RFQLineTypeController : BaseSSGController
    {
        #region Fields

        private readonly IRFQLineTypeService _rfqLineTypeService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;

        #endregion

        #region ctor

        public RFQLineTypeController(IRFQLineTypeService rfqLineTypeService,
            AdminAreaSettings adminAreaSettings,
            IWorkContext workContext,
            IPermissionService permissionService
            )
        {
            this._rfqLineTypeService = rfqLineTypeService;
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

            var query = this._rfqLineTypeService.GetAllRFQLineTypesQueryable();

            int totalCount = query.Count();

            query = query.OrderBy(s => s.DateUpdatedOnUtc).Skip((page.Value - 1) * _adminAreaSettings.GridPageSize).Take(_adminAreaSettings.GridPageSize);

            var supportCodes = from sc in query
                               select new RFQLineTypeModel
                               {
                                   Id = sc.Id,
                                   Name = sc.Name,
                                   Active = sc.Active
                               };

            var pagedList = new StaticPagedList<RFQLineTypeModel>(supportCodes, page.Value, _adminAreaSettings.GridPageSize, totalCount);

            if (Request.IsAjaxRequest())
            {
                return PartialView(MVC.RFQLineType.Views._Table, pagedList);
            }

            return View(pagedList);
        }

        public virtual ActionResult Create()
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var rfqLineType = new RFQLineTypeModel
            {
                Active = true
            };

            return View(MVC.RFQLineType.Views.CreateEdit, rfqLineType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(RFQLineTypeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rfqLineType = Mapper.Map<RFQLineType>(model);

                    rfqLineType.CreatedByUserId = _workContext.CurrentUser.Id;
                    rfqLineType.UpdatedByUserId = _workContext.CurrentUser.Id;
                    rfqLineType.DateCreatedOnUtc = DateTime.UtcNow;
                    rfqLineType.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._rfqLineTypeService.SaveRFQLineType(rfqLineType);
                    SuccessNotification("RFQ Line Type has been created.");
                    return RedirectToAction(MVC.RFQLineType.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.RFQLineType.Views.CreateEdit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var entity = this._rfqLineTypeService.GetRFQLineTypeById(id);

            var rfqLineType = Mapper.Map<RFQLineTypeModel>(entity);

            return View(MVC.RFQLineType.Views.CreateEdit, rfqLineType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(RFQLineTypeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rfqLineType = Mapper.Map<RFQLineType>(model);
                    rfqLineType.UpdatedByUserId = _workContext.CurrentUser.Id;
                    rfqLineType.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._rfqLineTypeService.SaveRFQLineType(rfqLineType);
                    SuccessNotification("RFQ Line Type has been saved.");
                    return RedirectToAction(MVC.RFQLineType.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.RFQLineType.Views.CreateEdit, model);
        }

        #endregion
    }
}