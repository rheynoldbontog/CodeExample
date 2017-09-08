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
using AutoMapper;
using SSG.Services.Directory;
using SSG.Web.Models.FineUploader;
using System.IO;
using SSG.Core.Domain.RFQ;
using FluentValidation.Mvc;
using SSG.Core.EntityExtensions;
using SSG.Services.Logging;
using SSG.Services.Users;
using SSG.Web.Extensions;

namespace SSG.Web.Controllers
{
    public partial class RFQLineController : BaseSSGController
    {
        #region Fields

        private readonly IRFQLineService _rfqLineService;
        private readonly IRFQService _rfqService;
        private readonly IUOMService _uomService;
        private readonly IRFQLineTypeService _rfqLineTypeService;
        private readonly IRFQLineFormTypeService _rfqLineFormTypeService;
        private readonly IRFQLineStatusService _rfqLineStatusService;
        private readonly IRFQLineStatusHistoryService _rfqLineStatusHistoryService;
        private readonly IEquipmentPurchaseTypeService _rfqLineEquipmentPurchaseTypeService;
        private readonly IEquipmentCalibrationTypeService _rfqLineEquipmentCalibrationTypeService;
        private readonly IFileAttachmentService _fileAttachmentService;
        private readonly IQuotationService _quotationService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;
        private readonly ICurrencyService _currencyService;
        private readonly ILogger _logger;
        
        #endregion

        #region ctor

        public RFQLineController(IRFQLineService rfqLineService,
            IRFQService rfqService,
            IUOMService uomService,
            IRFQLineTypeService rfqLineTypeService,
            IRFQLineFormTypeService rfqLineFormTypeService,
            IRFQLineStatusService rfqLineStatusService,
            IRFQLineStatusHistoryService rfqLineStatusHistoryService,
            IEquipmentPurchaseTypeService rfqLineEquipmentPurchaseTypeService,
            IEquipmentCalibrationTypeService rfqLineEquipmentCalibrationTypeService,
            ICurrencyService currencyService,
            IFileAttachmentService fileAttachmentService,
            IQuotationService quotationService,
            AdminAreaSettings adminAreaSettings,
            IWorkContext workContext,
            IPermissionService permissionService,
            ILogger logger
            )
        {
            this._rfqLineService = rfqLineService;
            this._rfqService = rfqService;
            this._uomService = uomService;
            this._rfqLineTypeService = rfqLineTypeService;
            this._rfqLineFormTypeService = rfqLineFormTypeService;
            this._rfqLineStatusService = rfqLineStatusService;
            this._rfqLineStatusHistoryService = rfqLineStatusHistoryService;
            this._rfqLineEquipmentPurchaseTypeService = rfqLineEquipmentPurchaseTypeService;
            this._rfqLineEquipmentCalibrationTypeService = rfqLineEquipmentCalibrationTypeService;
            this._currencyService = currencyService;
            this._fileAttachmentService = fileAttachmentService;
            this._quotationService = quotationService;
            this._adminAreaSettings = adminAreaSettings;
            this._workContext = workContext;
            this._permissionService = permissionService;
            this._logger = logger;
        }

        #endregion

        #region Enum

        enum rfqLineStatusEnum
        {
            None,
            Open,
            Closed,
            Deleted
        }

        #endregion
        #region Methods

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create([Bind(Exclude = "upload_url")] RFQLineModel rfqLine)
        {
            if (ModelState.IsValid)
            {
                var line = new RFQLine();

                //Map attachments and other fields...
                line.ToEntityForAdding(rfqLine, _rfqLineStatusService, _workContext.CurrentUser.Id);

                //Save RFQ to database...
                try
                {
                    _rfqLineService.SaveRFQLine(line);

                    string url = Url.Action("Lines", "RFQLine", new { id = rfqLine.RFQId });
                    return Json(new { success = true, url = url });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    _logger.Error(string.Format("Error in creating RFQ Line. {0}", ex.Message), ex);
                }
            }

            //If we got this far, something failed, redisplay form
            rfqLine.PopulateLookupsForLineModel(_uomService, _rfqLineTypeService, _rfqLineFormTypeService, _rfqLineEquipmentPurchaseTypeService, _rfqLineEquipmentCalibrationTypeService, _currencyService);

            if (rfqLine.Attachments == null)
                rfqLine.Attachments = new List<FileAttachmentModel>();

            return PartialView("_CreateEdit", rfqLine);
        }

        [HttpGet]
        public virtual JsonResult Delete(int lineId)
        {
            var rfqLine = _rfqLineService.GetRFQLineById(lineId);

            if (rfqLine != null)
            {
                if (rfqLine.UserCanDeleteLine(_workContext.CurrentUser))
                {
                    try
                    {
                        var oldStatusId = rfqLine.RFQLineStatusId;
                        var newStatusId = (int)RFQLineStates.Deleted;

                        rfqLine.RFQLineStatusId = newStatusId;
                        rfqLine.Active = false;
                        rfqLine.PopulateAuditFieldsForEdit(_workContext.CurrentUser.Id);

                        rfqLine.RFQ.SetEmailAttachmentFilePath(Server.MapPath("~/Email/RFQ/"));
                        //rfqLine.RFQ.EditUrl = Url.Action("Edit", "RFQ", new { id = rfqLine.RFQId }, this.Request.Url.Scheme);
                        rfqLine.RFQ.EditUrl = Url.Action("RemoteLogin", "User", new { userId = _workContext.CurrentUser.Id, returnUrl = Url.Action("Edit", "RFQ", new { id = rfqLine.RFQId }, this.Request.Url.Scheme) }, this.Request.Url.Scheme);

                        _rfqLineService.SaveRFQLine(rfqLine);

                        // Add change in status history...
                        _rfqLineStatusHistoryService.SaveStatusHistory(rfqLine.Id, oldStatusId, newStatusId, _workContext.CurrentUser.Id);

                    }
                    catch (Exception ex)
                    {
                        _logger.Error(string.Format("Error in updating RFQ Line. {0}", ex.Message), ex);
                        return Json(new { success = false, errorMessage = String.Format("Error in deleting line with ID: {0}!", lineId) }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json(new { success = false, errorMessage = String.Format("You must be the specified requestor or buyer inorder to delete a line!", lineId) }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, errorMessage = String.Format("RFQ Line with ID: {0} does not exist in the database!", lineId) }, JsonRequestBehavior.AllowGet);


            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult Edit([Bind(Exclude = "upload_url")] RFQLineModel model)
        {
            var existingRFQLine = _rfqLineService.GetRFQLineById(model.Id);
            
            if (ModelState.IsValid)
            {
                if (existingRFQLine != null)
                {
                    if (existingRFQLine.UserCanEditLine(_workContext.CurrentUser))
                    {
                        try
                        {
                            //Map from model...
                            existingRFQLine.ToEntityForEditing(model,
                                                     _fileAttachmentService,
                                                     _workContext.CurrentUser.Id,
                                                     Server.MapPath("~/Email/RFQ/"),
                                                     Url.Action("RemoteLogin", "User", new { userId = _workContext.CurrentUser.Id, returnUrl = Url.Action("Edit", "RFQ", new { id = existingRFQLine.RFQId }, this.Request.Url.Scheme) }, this.Request.Url.Scheme),
                                                     Server.MapPath("~/Attachments/Quotation/"),
                                                     Server.MapPath("~/Attachments/ROHS/"),
                                                     Server.MapPath("~/Attachments/MSDS/"),
                                                     Server.MapPath("~/Attachments/Other/"));
                            
                            _rfqLineService.SaveRFQLine(existingRFQLine);

                            string url = Url.Action("Lines", "RFQLine", new { id = existingRFQLine.RFQId });
                            return Json(new { success = true, url = url });
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", ex);
                            _logger.Error(string.Format("Error in updating RFQ Line. {0}", ex.Message), ex);
                        }
                }
                else
                    return AccessDeniedPage("You must be the specified requestor or buyer to edit an RFQ Line");
            }
            else
                return ResourceNotFound(String.Format("RFQ Line with ID: {0} not found in database!", model.Id));    
            }

            //If we got this far, something failed, redisplay form
            model.PopulateLookupsForLineModel(_uomService, _rfqLineTypeService, _rfqLineFormTypeService, _rfqLineEquipmentPurchaseTypeService, _rfqLineEquipmentCalibrationTypeService, _currencyService);

            
            return PartialView(MVC.RFQLine.Views._BuyerEdit, model);
        }

        public virtual ActionResult Edit(int rfqLineId)
        {
            var rfqLine = _rfqLineService.GetRFQLineById(rfqLineId);
            var rfqVm = new RFQLineModel();

            if (rfqLine != null)
            {
                if (rfqLine.UserCanEditLine(_workContext.CurrentUser))
                {
                    rfqVm = Mapper.Map<RFQLineModel>(rfqLine);
                    rfqVm.ToModelForEditing(rfqLine,
                                            _currencyService,
                                            _uomService,
                                            _rfqLineTypeService,
                                            _rfqLineFormTypeService,
                                            _rfqLineEquipmentPurchaseTypeService,
                                            _rfqLineEquipmentCalibrationTypeService,
                                            rfqLine.RFQ.UserIsBuyer(_workContext.CurrentUser),
                                            rfqLine.RFQ.UserIsRequestor(_workContext.CurrentUser));

                    return PartialView(MVC.RFQLine.Views._BuyerEdit, rfqVm);
                }
                else
                    return AccessDeniedPage("You must be the specified requestor or buyer to edit an RFQ Line");
                
            }
            else
                return ResourceNotFound(String.Format("RFQ Line with ID: {0} not found in database!", rfqLineId));
            
        }

        public virtual ActionResult Create(int rfqId, int lineCount)
        {
            var rfq = _rfqService.GetById(rfqId);

            if (rfq != null)
            {
                if (rfq.UserCanAddLine(_workContext.CurrentUser))
                {
                    var rfqLine = new RFQLineModel
                    {
                        RFQLineNo = lineCount + 1,
                        RFQId = rfqId,
                        Attachments = new List<FileAttachmentModel>(),
                        RFQCreationDate = rfq.DateCreatedOnUtc,
                        RFQStatus = rfq.RFQStatus.Name,
                        RFQIsSubmitted = false,
                        UserIsBuyer = rfq.UserIsBuyer(_workContext.CurrentUser),
                        UserIsRequestor = rfq.UserIsRequestor(_workContext.CurrentUser),
                    };

                    rfqLine.PopulateLookupsForLineModel(_uomService, _rfqLineTypeService, _rfqLineFormTypeService, _rfqLineEquipmentPurchaseTypeService, _rfqLineEquipmentCalibrationTypeService, _currencyService, true);

                    return PartialView("_CreateEdit", rfqLine);
                }
                else
                    return AccessDeniedPage("You must be the specified requestor to add a line in an RFQ");
            }
            else
                return ResourceNotFound(String.Format("RFQ with ID: {0} not found in database!", rfqId));
        }

        public virtual ActionResult Lines(int id)
        {
            var rfqLines = _rfqLineService.GetAllRFQLinesQueryable(true).Where(x => x.RFQId == id).OrderBy(x => x.RFQLineNo).ToList();
            var rfqLinesVm = new List<RFQLineModel>();

            //Lines
            foreach (var line in rfqLines)
            {
                var lineVm = Mapper.Map<RFQLineModel>(line);

                lineVm.Attachments = new List<FileAttachmentModel>();

                foreach (var attachment in line.Attachments)
                {
                    lineVm.Attachments.Add(Mapper.Map<FileAttachmentModel>(attachment));
                }

                rfqLinesVm.Add(lineVm);
            }

            return PartialView("_Lines", rfqLinesVm);
        }

        [HttpPost]
        public virtual FineUploaderResult UploadRFQLineAttachment(FineUpload upload)
        {
            var dir = Server.MapPath("~/Attachments/RFQLine/");
            var uuid = HttpContext.Request["qquuid"];

            return SaveUploadedFile(upload, dir, uuid);
        }

        private FineUploaderResult SaveUploadedFile(FineUpload upload, string dir, string uuid)
        {
            var filePath = Path.Combine(dir, upload.Filename);
            try
            {
                upload.SaveAs(filePath, true, true);
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }

            var fileName = upload.Filename;
            var fileUrl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~/Attachments/RFQLine/") + upload.Filename;

            // the anonymous object in the result below will be convert to json and set back to the browser
            return new FineUploaderResult(true, new { filename = fileName, fileurl = fileUrl });
        }


        public virtual ActionResult ViewLines(int id)
        {
            var rfqLines = _rfqLineService.GetAllRFQLinesQueryable(true).Where(x => x.RFQId == id).OrderBy(x => x.RFQLineNo).ToList();
            var rfqLinesVm = new List<RFQLineModel>();

            foreach (var line in rfqLines)
            {
                var lineVm = Mapper.Map<RFQLineModel>(line);

                lineVm.Attachments = new List<FileAttachmentModel>();

                foreach (var attachment in line.Attachments)
                {
                    lineVm.Attachments.Add(Mapper.Map<FileAttachmentModel>(attachment));
                }

                rfqLinesVm.Add(lineVm);
            }

            return PartialView("_ViewLines", rfqLinesVm);
        }
        [HttpPost]
        public virtual ActionResult ViewLinesDetails(int id)
        {
            var query = _rfqLineService.GetRFQLineById(id);
            var model = new RFQLineModel();
            model.Id = query.Id;
            model.RFQLineNo = query.RFQLineNo;
            model.ItemDescription = query.ItemDescription;
            model.Quantity = query.Quantity;
            model.QuantityUnit = query.QuantityUnit.Name;
            model.Maker = query.Maker;
            model.MakerPN = query.MakerPN;
            model.ReferenceLinks = query.ReferenceLinks;
            model.ROHSCompliant = query.ROHSCompliant;
            model.OtherTechnicalDetails = query.OtherTechnicalDetails;
            model.NoteToBuyer = query.NoteToBuyer;
            model.FirstUploadDate = query.FirstUploadDate;
            model.TestEquipmentPurchaseType = query.FormType.Id == 2 ?query.TestEquipmentPurchaseType.Name:"";
            model.TestEquipmentCalibrationType = query.FormType.Id == 2 ?query.TestEquipmentCalibrationType.Name:"";
            model.TestEquipmentApplication = query.FormType.Id == 2 ?query.TestEquipmentApplication:"";
            model.Active = query.Active;
            model.LineTypeId = query.LineType.Id;
            model.LineType = query.LineType.Name;
            model.FormTypeId = query.FormType.Id;
            model.FormType = query.FormType.Name;
            model.Status = query.Status.Name;
            model.DateCreatedOnUtc = query.DateCreatedOnUtc;
            foreach (var q in query.Attachments)
            {
                model.Attachments = new List<FileAttachmentModel>();
                model.Attachments.Add(Mapper.Map<FileAttachmentModel>(q));
            }
            return PartialView(MVC.RFQLine.Views._ViewLineDetails,model);

        }
        #endregion
    }
}