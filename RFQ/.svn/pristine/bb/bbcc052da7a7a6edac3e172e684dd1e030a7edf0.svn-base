using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using AutoMapper;
using SSG.Core.Domain.RFQ;
using SSG.Services.RFQ;
using SSG.Web.Framework;
using SSG.Web.Models.RFQ;
using SSG.Web.Extensions;
using SSG.Core;
using SSG.Services.Users;
using SSG.Services.Directory;
using SSG.Web.Models.FineUploader;
using System.IO;
using System.Web.Hosting;

namespace SSG.Web.Controllers
{
    public partial class QuotationController : BaseSSGController
    {
        //
        // GET: /Quotation/

        private readonly IQuotationService _quotationService;
        private readonly IRFQLineService _rfqLineService;
        private readonly IReportService _reportService;
        private readonly IWorkContext _workContext;
        private readonly ICurrencyService _currencyService;
        public QuotationController(
            IQuotationService quotationService,
            IRFQLineService rfqLineService,
            IReportService reportService,
            IWorkContext workContext,
            ICurrencyService currencyService
            )
        {
            this._quotationService = quotationService;
            this._rfqLineService = rfqLineService;
            this._reportService = reportService;
            this._workContext = workContext;
            this._currencyService = currencyService;
        }
        public virtual ActionResult Index()
        {
            return View();
        }

        //private void PopulateLookups(QuotationModel quotationVm)
        //{
        //    var currencies = _currencyService.GetAllCurrencies(false);
        //    quotationVm.AvailableCurrencies = new List<SelectListItem>();

        //    foreach (var currency in currencies)
        //    {
        //        quotationVm.AvailableCurrencies.Add(new SelectListItem() { Text = currency.CurrencyCode, Value = currency.Id.ToString() });
        //    }

        //    var defaultCurrency = _currencyService.GetCurrencyByCode("PHP");

        //    if (defaultCurrency != null)
        //    {
        //        quotationVm.CurrencyId = defaultCurrency.Id;
        //        quotationVm.Currency = defaultCurrency.CurrencyCode;
        //    }
        //}

        public virtual JsonResult PaymentTermsLookup(string query)
        {
            var paymentTerms = _quotationService.GetAllQuotations().Where(x => x.PaymentTerms.Trim().ToUpper().StartsWith(query.Trim().ToUpper())).Select(x => x.PaymentTerms).Distinct().ToList();

            return Json(new { options = paymentTerms }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult SupplierLookup(string query)
        {
            var suppliers = _quotationService.GetAllQuotations().Where(x => x.Supplier.Trim().ToUpper().StartsWith(query.Trim().ToUpper())).Select(x => x.Supplier).Distinct().ToList();

            return Json(new { options = suppliers }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult CommodityLookup(string query)
        {
            var commodities = _quotationService.GetAllQuotations().Where(x => x.Commodity.Trim().ToUpper().StartsWith(query.Trim().ToUpper())).Select(x => x.Commodity).Distinct().ToList();

            return Json(new { options = commodities }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult Create(int rfqLineId, int lastIndex)
        {
            var rfqLine = _rfqLineService.GetRFQLineById(rfqLineId);

            if (rfqLine != null)
            {
                if (rfqLine.RFQ.UserIsBuyer(_workContext.CurrentUser))
                {
                    var quotationVm = new QuotationModel()
                    {
                        RFQLineId = rfqLineId,
                        Index = lastIndex++.ToString(),
                        QuotationNo = lastIndex,
                        QuoteItemDescription = rfqLine.ItemDescription,
                        MakerPN = rfqLine.MakerPN,
                        IsDeleted = false,
                        UserIsBuyer = rfqLine.RFQ.UserIsBuyer(_workContext.CurrentUser),
                        UserIsRequestor = rfqLine.RFQ.UserIsRequestor(_workContext.CurrentUser)
                    };

                    quotationVm.PopulateLookupsForQuotationModel(_currencyService);

                    var tabHeaderItm = this.ControllerContext.RenderPartialToString("_QuotationTabHeaderItem", quotationVm);
                    var tabContentItm = this.ControllerContext.RenderPartialToString("_QuotationTabContentItem", quotationVm);

                    tabContentItm = string.Format(tabContentItm, quotationVm.QuotationNo);

                    return Json(new { success = true, tabHeaderItem = tabHeaderItm, tabContentItem = tabContentItm }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { success = false, errorMessage = "Sorry, only buyer of this RFQ can create a quotation for it!" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, errorMessage = "Error in getting RFQLine from database!" }, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult Quotations(RFQLineModel model)
        {
            return PartialView("_Quotations", model);
        }

        [HttpPost]
        public virtual ActionResult QuotationMatrix(int rfqLineId)
        {
            var query = _quotationService.GetAllQuotations().Where(x => x.RFQLineId == rfqLineId);
            var result = (from q in query
                          select new QuotationModel()
                          {
                              QuotationNo = q.QuotationNo,
                              QuoteUploadDate = q.QuoteUploadDate,
                              Commodity = q.Commodity,
                              QuoteValidity = q.QuoteValidity,
                              QuoteItemDescription = q.QuoteItemDescription,
                              MakerPN = q.MakerPN,
                              UnitPrice = q.UnitPrice,
                              DiscountedPrice = q.DiscountedPrice,
                              MinimumOrderQuantity = q.MinimumOrderQuantity,
                              PaymentTerms = q.PaymentTerms,
                              Supplier = q.Supplier,
                              DeliveryLeadTime = q.DeliveryLeadTime,
                              WarrantyPeriod = q.WarrantyPeriod,
                              BuyerRemarks = q.BuyerRemarks,
                              RequesterRemarks = q.RequesterRemarks,
                              Active = q.Active,
                              RFQLineId = q.RFQLineId,

                          }).ToList();

            var queryRfqLine = _rfqLineService.GetRFQLineById(rfqLineId);

            var rfqLineIndexModel = new RFQLineIndexModel
            {
                RFQNo = queryRfqLine.RFQ.RFQNo,
                RFQLineID = queryRfqLine.Id,
                RFQLineNo = queryRfqLine.RFQLineNo,
                RFQCreationDate = queryRfqLine.DateCreatedOnUtc
            };
            var model = new QuotationViewModel()
            {
                RfqLineIndexModel = rfqLineIndexModel,
                QuotationModel = result
            };
            return View(MVC.Quotation.Views._QuotationMatrix, model);
        }

        public virtual ActionResult ExportExcelQuotation(int id)
        {
            var query = _rfqLineService.GetRFQLineById(id);

            string fileName = string.Format("quotation_report_{0}_{1}.xlsx", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), CommonHelper.GenerateRandomDigitCode(3));
            string filePath = System.IO.Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "content\\files\\ExportImport", fileName);

            _reportService.ExportQuotation(filePath, query);

            var bytes = System.IO.File.ReadAllBytes(filePath);

            return File(bytes, "text/xls", fileName);
        }

    }
}
