using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSG.Core.Domain.Common;
using SSG.Core;
using SSG.Services.Security;
using SSG.Services.PSP;
using SSG.Web.Models;
using PagedList;
using SSG.Web.Models.PSP;
using SSG.Core.Domain.PSP;
using AutoMapper;

namespace SSG.Web.Controllers
{
    public partial class SupportCodeController : BaseSSGController
    {
        #region Fields

        private readonly ISupportCodeService _supportCodeService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;

        #endregion

        #region ctor
        
        public SupportCodeController(ISupportCodeService supportCodeService,
            AdminAreaSettings adminAreaSettings,
            IWorkContext workContext,
            IPermissionService permissionService
            )
        {
            this._supportCodeService = supportCodeService;
            this._adminAreaSettings = adminAreaSettings;
            this._workContext = workContext;
            this._permissionService = permissionService;
        }


        #endregion

        #region Methods

        //public virtual ActionResult Index()
        //{
        //    return View();
        //}


        public virtual ActionResult Index(int? page)
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            page = page ?? 1;

            var query = this._supportCodeService.GetAllSupportCodeQueryable();

            int totalCount = query.Count();

            query = query.OrderBy(s => s.DateUpdatedOnUtc).Skip((page.Value - 1) * _adminAreaSettings.GridPageSize).Take(_adminAreaSettings.GridPageSize);

            var supportCodes = from sc in query
                        select new SupportCodeModel
                        {
                            Id = sc.Id,
                            SupportCodes = sc.SupportCodes,
                            SupportCodeDescription = sc.SupportCodeDescription,
                            Active = sc.Active
                        };

            var pagedList = new StaticPagedList<SupportCodeModel>(supportCodes, page.Value, _adminAreaSettings.GridPageSize, totalCount);

            if (Request.IsAjaxRequest())
            {
                return PartialView(MVC.SupportCode.Views._Table, pagedList);
            }

            return View(pagedList);
        }

        public virtual ActionResult Create()
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var supportCode = new SupportCodeModel
            {
                Active = true
            };

            return View(MVC.SupportCode.Views.CreateEdit, supportCode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(SupportCodeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var supportCode = Mapper.Map<SupportCode>(model);
                    //var supportCode = new SupportCode();
                    //supportCode.Id = model.Id;
                    //supportCode.SupportCodes = model.SupportCodes;
                    //supportCode.SupportCodeDescription = model.SupportCodeDescription;

                    supportCode.CreatedByUserId = _workContext.CurrentUser.Id;
                    supportCode.UpdatedByUserId = _workContext.CurrentUser.Id;
                    supportCode.DateCreatedOnUtc = DateTime.UtcNow;
                    supportCode.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._supportCodeService.SaveSupportCode(supportCode);
                    SuccessNotification("SupportCode has been created.");
                    return RedirectToAction(MVC.SupportCode.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.SupportCode.Views.CreateEdit, model);
        }

        public virtual ActionResult Edit(int id)
        {
            if (!(_permissionService.Authorize(StandardPermissionProvider.MaintenanceNavigation) || _workContext.IsAdmin))
                return AccessDenied();

            var entity = this._supportCodeService.GetSupportCodeById(id);

            var supportCode = Mapper.Map<SupportCodeModel>(entity);

            return View(MVC.SupportCode.Views.CreateEdit, supportCode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SupportCodeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var supportCode = Mapper.Map<SupportCode>(model);
                    supportCode.UpdatedByUserId = _workContext.CurrentUser.Id;
                    supportCode.DateUpdatedOnUtc = DateTime.UtcNow;
                    this._supportCodeService.SaveSupportCode(supportCode);
                    SuccessNotification("Support Code has been saved.");
                    return RedirectToAction(MVC.SupportCode.Index());
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return View(MVC.SupportCode.Views.CreateEdit, model);
        }

        #endregion
    }
}
