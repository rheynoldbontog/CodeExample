using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using PagedList;
using SSG.Core;
using SSG.Core.Domain.Common;
using SSG.Core.Domain.RFQ;
using SSG.Core.Domain.Users;
using SSG.Services.Directory;
using SSG.Services.RFQ;
using SSG.Services.Security;
using SSG.Services.Users;
using SSG.Web.Models.RFQ;
using System.Data.Objects;
using System.Globalization;
using System.IO;
using System.Web.Hosting;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using SSG.Web.Framework;
using Telerik.Web.Mvc.Extensions;
using WebGrease.Css.Extensions;

namespace SSG.Web.Controllers
{
    public partial class RFQIndexController : BaseSSGController
    {
        #region Fields

        private readonly IRFQLineService _rfqLineService;
        private readonly IRFQService _rfqService;
        private readonly IUOMService _uomService;
        private readonly IRFQLineTypeService _rfqLineTypeService;
        private readonly IRFQLineFormTypeService _rfqLineFormTypeService;
        private readonly IEquipmentPurchaseTypeService _rfqLineEquipmentPurchaseTypeService;
        private readonly IEquipmentCalibrationTypeService _rfqLineEquipmentCalibrationTypeService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;
        private readonly ICurrencyService _currencyService;
        private readonly IUserService _userService;
        private readonly IRFQLineStatusService _rfqLineStatusService;
        private readonly IReportService _reportService;
        #endregion
        // GET: /RFQIndex/
        public RFQIndexController(
            IRFQLineService rfqLineService,
            IRFQService rfqService,
            IUOMService uomService,
            IRFQLineTypeService rfqLineTypeService,
            IRFQLineFormTypeService rfqLineFormTypeService,
            IEquipmentPurchaseTypeService rfqLineEquipmentPurchaseTypeService,
            IEquipmentCalibrationTypeService rfqLineEquipmentCalibrationTypeService,
            ICurrencyService currencyService,
            AdminAreaSettings adminAreaSettings,
            IWorkContext workContext,
            IPermissionService permissionService,
            IUserService userService,
            IRFQLineStatusService rfqLineStatusService,
            IReportService reportService
        )
        {
            this._rfqLineService = rfqLineService;
            this._rfqService = rfqService;
            this._uomService = uomService;
            this._rfqLineTypeService = rfqLineTypeService;
            this._rfqLineFormTypeService = rfqLineFormTypeService;
            this._rfqLineEquipmentPurchaseTypeService = rfqLineEquipmentPurchaseTypeService;
            this._rfqLineEquipmentCalibrationTypeService = rfqLineEquipmentCalibrationTypeService;
            this._currencyService = currencyService;
            this._adminAreaSettings = adminAreaSettings;
            this._workContext = workContext;
            this._permissionService = permissionService;
            this._userService = userService;
            this._rfqLineStatusService = rfqLineStatusService;
            this._reportService = reportService;
        }

        public IQueryable<RFQLineService.TableJoin> Query(RFQLineModelIndexes rfqLineModelIndexes, bool isUsingSearch)
        {
            var query = _rfqLineService.GetAllQueryableTableJoins();
            if (_workContext.CurrentUser.IsRequestor(true))
                query = query.Where(x => x.Rfq.RequestorId == _workContext.CurrentUser.Id || x.Rfq.Requestor.DepartmentId == _workContext.CurrentUser.DepartmentId);

            if (_workContext.CurrentUser.IsBuyer(true))
            {
                query = query.Where(x => x.Rfq.RFQStatusId == (int)RFQStates.Submitted);
                query = query.Where(x => x.Rfq.BuyerId == _workContext.CurrentUser.Id || x.Rfq.Buyer.DepartmentId == _workContext.CurrentUser.DepartmentId);
            }
            
            if (isUsingSearch == true)
            {
                
                if (!string.IsNullOrEmpty(rfqLineModelIndexes.RfqLineIndex.RFQNo))
                {
                    query = query.Where(x => x.Rfq.RFQNo == rfqLineModelIndexes.RfqLineIndex.RFQNo);
                }
                if (!string.IsNullOrEmpty(rfqLineModelIndexes.RfqLineIndex.Commodity))
                {
                    query = query.Where(x => x.RfqLine.Quotations.Where(q => q.Commodity.Contains(rfqLineModelIndexes.RfqLineIndex.Commodity)).Any());
                }
                if (rfqLineModelIndexes.RfqLineModel.StatusId > 0)
                {
                    query = query.Where(x => x.RfqLine.RFQLineStatusId == rfqLineModelIndexes.RfqLineModel.StatusId);
                }
                if (rfqLineModelIndexes.RfqModel.BuyerId > 0)
                {
                    query = query.Where(x => x.Rfq.BuyerId == rfqLineModelIndexes.RfqModel.BuyerId);
                }
                if (rfqLineModelIndexes.StartDate != null || rfqLineModelIndexes.EndDate != null)
                {
                    DateTime startDateTime = new DateTime(rfqLineModelIndexes.StartDate.Value.Year,
                        rfqLineModelIndexes.StartDate.Value.Month, rfqLineModelIndexes.StartDate.Value.Day, 0, 0, 0, 0);

                    DateTime endDateTime = new DateTime(rfqLineModelIndexes.EndDate.Value.Year, rfqLineModelIndexes.EndDate.Value.Month, rfqLineModelIndexes.EndDate.Value.Day, 23, 59, 59,
                        999);

                    query = query.Where(x => x.Rfq.DateCreatedOnUtc >= startDateTime && x.Rfq.DateCreatedOnUtc <= endDateTime);
                }

                if (!string.IsNullOrEmpty(rfqLineModelIndexes.SearchOthers))
                {
                    int rfqLineNo;
                    bool result = int.TryParse(rfqLineModelIndexes.SearchOthers, out rfqLineNo);

                    if (result)
                    {
                        query = query.Where(x => x.RfqLine.RFQLineNo == rfqLineNo);
                    }
                    else
                    {
                        query = query
                            .Where(x =>
                            x.RfqLine.ItemDescription.Contains(rfqLineModelIndexes.SearchOthers) ||
                            x.RfqLine.MakerPN.Contains(rfqLineModelIndexes.SearchOthers) ||
                            x.Rfq.Department.Contains(rfqLineModelIndexes.SearchOthers) ||
                            x.RfqLine.FormType.Name.Contains(rfqLineModelIndexes.SearchOthers) ||
                            x.RfqLine.TestEquipmentPurchaseType.Name.Contains(rfqLineModelIndexes.SearchOthers) ||
                            x.RfqLine.TestEquipmentCalibrationType.Name.Contains(rfqLineModelIndexes.SearchOthers) ||
                            x.RfqLine.TestEquipmentApplication.Contains(rfqLineModelIndexes.SearchOthers) ||
                            x.Rfq.Subject.Contains(rfqLineModelIndexes.SearchOthers)
                        );
                    }
                    
                }
                return query;

            }
            else
            {
                return query;
            }
        }

        public static IQueryable<RFQLineService.TableJoin> TableJoins{ get; set; }
        public virtual ActionResult Index(RFQLineModelIndexes rfqLineModelIndexes, int? page)
        {
            if (!_workContext.CurrentUser.IsRegistered())
            {
                return View(MVC.Home.Views.Index);
            }

            bool isUsingSearch = (Request.IsAjaxRequest()) ? true : false;

            page = page ?? 1;

            var query = Query(rfqLineModelIndexes, isUsingSearch);
                
          
            // Do paging now...
            query = query.OrderByDescending(x => x.Rfq.DateCreatedOnUtc).Skip((page.Value - 1) * _adminAreaSettings.GridPageSize).Take(_adminAreaSettings.GridPageSize);
            
            // This one is an error so do we need this?
            //.ToList();

            var rfq = (from r in query.ToList()
                       select new PageIndex()
                       {
                           RfqModel = Mapper.Map<RFQModel>(r.Rfq),
                           RfqLineModel = Mapper.Map<RFQLineModel>(r.RfqLine)
                       }
            );
           
            var newRfq = rfq.ToList();

            foreach (var r in newRfq)
            {
                if (r.RfqLineModel != null)
                {
                    var rfqLine = _rfqLineService.GetRFQLineById(r.RfqLineModel.Id);
                    r.RfqLineModel.IsUserCanEditable = rfqLine.UserCanEditLine(_workContext.CurrentUser);
                    r.RfqLineModel.IsUserCanDeletable = rfqLine.UserCanDeleteLine(_workContext.CurrentUser);
                }

                var existingRfq = _rfqService.GetById(r.RfqModel.Id);

                if (existingRfq != null)
                    r.RfqModel.UserCanEdit = existingRfq.UserCanEdit(_workContext.CurrentUser);
                else
                    r.RfqModel.UserCanEdit = false;

                r.RfqModel.Requester = _userService.GetUserById(r.RfqModel.RequesterId).GetFullName();
                r.RfqModel.Buyer = _userService.GetUserById(r.RfqModel.BuyerId).GetFullName();

            }

            int totalCount = query.Count();
            var pagedList = new StaticPagedList<PageIndex>(newRfq, page.Value, _adminAreaSettings.GridPageSize, totalCount);

            if (Request.IsAjaxRequest())
            {
                return View(MVC.RFQIndex.Views._Tables, pagedList);
            }
            var rfqLineModel = new RFQLineModel();
            var status = _rfqLineStatusService.GetAllStatus(false);
            rfqLineModel.StatusSelectListItems = new List<SelectListItem>();

            foreach (var stats in status)
            {
                rfqLineModel.StatusSelectListItems.Add(new SelectListItem() { Text = stats.Name, Value = stats.Id.ToString() });
            }

            // Buyers List...
            var rfqModel = new RFQModel();
            var buyersRoleIds = new[] { _userService.GetUserRoleBySystemName(SystemUserRoleNames.Buyer).Id };
            var buyers = _userService.GetUsersByUserRoleId(buyersRoleIds[0]).ToList();

            foreach (var buyer in buyers)
            {
                rfqModel.BuyersList.Add(new SelectListItem() { Text = buyer.GetFullName(), Value = buyer.Id.ToString() });
            }

            var model = new RFQLineModelIndexes()
            {
                PageIndex = pagedList,
                RfqLineModel = rfqLineModel,
                RfqModel = rfqModel
            };

            return View(model);
        }
        [HttpPost]
        public virtual ActionResult ExportToExcel(RFQLineModelIndexes rfqLineModelIndexes)
        {
            var query = Query(rfqLineModelIndexes, true);

            string fileName = string.Format("rfqAll_report_{0}_{1}.xlsx", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), CommonHelper.GenerateRandomDigitCode(3));
            string filePath = System.IO.Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "content\\files\\ExportImport", fileName);

            _reportService.ExportAllRFQIndex(filePath,query.ToList());

            return new JsonResult()
            {
                Data = new {FileName = fileName,FilePath = filePath}
            };
        }
        [HttpGet]
        public virtual ActionResult Download(string filePath,string fileName)
        {
            var bytes = System.IO.File.ReadAllBytes(filePath);

            return File(bytes, "text/xls", fileName);
        }
    }
}