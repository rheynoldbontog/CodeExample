using SSG.Core.Domain.Common;
using SSG.Core.Domain.Directory;
using SSG.Core.Domain.Localization;
using SSG.Core.Domain.Topics;
using SSG.Services.Localization;
using SSG.Services.Seo;
using SSG.Web.Models.Common;
using SSG.Web.Models.Topics;
using SSG.Core.Domain.RFQ;
using SSG.Web.Models.RFQ;
using System;
using SSG.Services.RFQ;
using System.Collections.Generic;
using AutoMapper;
using SSG.Core.EntityExtensions;
using System.Linq;
using System.Web.Mvc;
using SSG.Services.Directory;
using SSG.Services.Users;
using SSG.Core.Domain.Users;

namespace SSG.Web.Extensions
{
    public static class MappingExtensions
    {

        //language
        public static LanguageModel ToModel(this Language entity)
        {
            if (entity == null)
                return null;

            var model = new LanguageModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                FlagImageFileName = entity.FlagImageFileName,
            };
            return model;
        }


        //currency
        public static CurrencyModel ToModel(this Currency entity)
        {
            if (entity == null)
                return null;

            var model = new CurrencyModel()
            {
                Id = entity.Id,
                Name = entity.GetLocalized(x => x.Name),
            };
            return model;
        }

        //address
        public static AddressModel ToModel(this Address entity)
        {
            if (entity == null)
                return null;

            var model = new AddressModel()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                Company = entity.Company,
                CountryId = entity.CountryId,
                CountryName = entity.Country != null ? entity.Country.GetLocalized(x => x.Name) : null,
                StateProvinceId = entity.StateProvinceId,
                StateProvinceName = entity.StateProvince != null ? entity.StateProvince.GetLocalized(x => x.Name) : null,
                City = entity.City,
                Address1 = entity.Address1,
                Address2 = entity.Address2,
                ZipPostalCode = entity.ZipPostalCode,
                PhoneNumber = entity.PhoneNumber,
                FaxNumber = entity.FaxNumber,
            };
            return model;
        }
        public static Address ToEntity(this AddressModel model)
        {
            if (model == null)
                return null;

            var entity = new Address();
            return ToEntity(model, entity);
        }
        public static Address ToEntity(this AddressModel model, Address destination)
        {
            if (model == null)
                return destination;

            destination.Id = model.Id;
            destination.FirstName = model.FirstName;
            destination.LastName = model.LastName;
            destination.Email = model.Email;
            destination.Company = model.Company;
            destination.CountryId = model.CountryId;
            destination.StateProvinceId = model.StateProvinceId;
            destination.City = model.City;
            destination.Address1 = model.Address1;
            destination.Address2 = model.Address2;
            destination.ZipPostalCode = model.ZipPostalCode;
            destination.PhoneNumber = model.PhoneNumber;
            destination.FaxNumber = model.FaxNumber;

            return destination;
        }

        //topics
        public static TopicModel ToModel(this Topic entity)
        {
            if (entity == null)
                return null;

            var model = new TopicModel()
            {
                Id = entity.Id,
                SystemName = entity.SystemName,
                IncludeInSitemap = entity.IncludeInSitemap,
                IsPasswordProtected = entity.IsPasswordProtected,
                Title = entity.GetLocalized(x => x.Title),
                Body = entity.GetLocalized(x => x.Body),
                MetaKeywords = entity.GetLocalized(x => x.MetaKeywords),
                MetaDescription = entity.GetLocalized(x => x.MetaDescription),
                MetaTitle = entity.GetLocalized(x => x.MetaTitle),
            };
            return model;
        }

        #region RFQLine

        public static void CheckIfFirstQuotationUpload(this RFQLine existingRFQLine, RFQLineModel model)
        {
            var newQuotationsCount = model.Quotations.Where(x => x.IsDeleted == false && x.Id == 0).Count();

            if (newQuotationsCount > 0 && existingRFQLine.Quotations.Count == 0)
                existingRFQLine.HasFirstUpload = true;
            else
                existingRFQLine.HasFirstUpload = false;
        }

        public static void PopulateLookupsForQuotationModel(this QuotationModel quotation,
                                                            ICurrencyService currencyService)
        {
            var currencies = currencyService.GetAllCurrencies(false);
            quotation.AvailableCurrencies = new List<SelectListItem>();

            foreach (var currency in currencies)
            {
                quotation.AvailableCurrencies.Add(new SelectListItem() { Text = currency.CurrencyCode, Value = currency.Id.ToString() });
            }

            var defaultCurrency = currencyService.GetCurrencyByCode("PHP");

            if (defaultCurrency != null)
            {
                quotation.CurrencyId = defaultCurrency.Id;
                quotation.Currency = defaultCurrency.CurrencyCode;
            }
        }


        public static void PopulateLookupsForReassignBuyer(this ReassignBuyerModel model,
                                                           IUserService userService)
        {
            var buyersRoleIds = new[] { userService.GetUserRoleBySystemName(SystemUserRoleNames.Buyer).Id };
            var buyers = userService.GetUsersByUserRoleId(buyersRoleIds[0]).ToList();

            foreach (var buyer in buyers)
            {
                model.BuyersList.Add(new SelectListItem() { Text = buyer.GetFullName(), Value = buyer.Id.ToString() });
            }


        }

        public static void PopulateLookupsForLineModel(this RFQLineModel rfqLine, 
                                                       IUOMService uomService, 
                                                       IRFQLineTypeService rfqLineTypeService, 
                                                       IRFQLineFormTypeService rfqLineFormTypeService,
                                                       IEquipmentPurchaseTypeService equipmentPurchaseTypeService,
                                                       IEquipmentCalibrationTypeService equipmentCalibrationTypeService,
                                                       ICurrencyService currencyService,
                                                       bool setDefaultTypes = false)
        {
            //Available Units
            var units = uomService.GetAllUOMsQueryable(true).ToList();
            rfqLine.AvailableUnits = new List<SelectListItem>();

            foreach (var unit in units)
            {
                rfqLine.AvailableUnits.Add(new SelectListItem() { Text = unit.Name, Value = unit.Id.ToString() });
            }

            //Available Line Types
            var lineTypes = rfqLineTypeService.GetAllRFQLineTypesQueryable(true).ToList();
            rfqLine.AvailableLineTypes = new List<SelectListItem>();

            foreach (var lineType in lineTypes)
            {
                rfqLine.AvailableLineTypes.Add(new SelectListItem() { Text = lineType.Name, Value = lineType.Id.ToString() });
            }

            //Available Line Form Types
            var lineFormTypes = rfqLineFormTypeService.GetAllRFQLineFormTypesQueryable(true).ToList();
            rfqLine.AvailableFormTypes = new List<SelectListItem>();

            foreach (var lineFormType in lineFormTypes)
            {
                rfqLine.AvailableFormTypes.Add(new SelectListItem() { Text = lineFormType.Name, Value = lineFormType.Id.ToString() });
            }

            //Available Equipment Purchase Types
            var equipmentPurchaseTypes = equipmentPurchaseTypeService.GetAllEquipmentPurchaseTypeQueryable(true).ToList();
            rfqLine.AvailableEquipmentPurchaseTypes = new List<SelectListItem>();

            foreach (var purchaseType in equipmentPurchaseTypes)
            {
                rfqLine.AvailableEquipmentPurchaseTypes.Add(new SelectListItem() { Text = purchaseType.Name, Value = purchaseType.Id.ToString() });
            }

            //Available Equipment Calibration Types
            var equipmentCalibrationTypes = equipmentCalibrationTypeService.GetAllEquipmentCalibrationTypeQueryable(true).ToList();
            rfqLine.AvailableEquipmentCalibrationTypes = new List<SelectListItem>();

            foreach (var calibrationType in equipmentCalibrationTypes)
            {
                rfqLine.AvailableEquipmentCalibrationTypes.Add(new SelectListItem() { Text = calibrationType.Name, Value = calibrationType.Id.ToString() });
            }

            if (setDefaultTypes)
            {
                var defaultLineType = rfqLineTypeService.GetRFQLineTypeById((int)RFQLineTypes.Goods);

                if (defaultLineType != null)
                {
                    rfqLine.LineTypeId = defaultLineType.Id;
                    rfqLine.LineType = defaultLineType.Name;
                }

                var defaultLineFormType = rfqLineFormTypeService.GetRFQLineFormTypeById((int)RFQLineFormTypes.GeneralForm);

                if (defaultLineFormType != null)
                {
                    rfqLine.FormTypeId = defaultLineFormType.Id;
                    rfqLine.FormType = defaultLineFormType.Name;
                }
            }

            //Quotation lookups...
            if (rfqLine.Quotations != null)
            {
                foreach (var quotation in rfqLine.Quotations)
                {
                    quotation.PopulateLookupsForQuotationModel(currencyService);
                }
            }
        }

        public static void ToModelForEditing(this RFQLineModel model, 
                                             RFQLine rfqLine,
                                             ICurrencyService currencyService,
                                             IUOMService uomService, 
                                             IRFQLineTypeService rfqLineTypeService, 
                                             IRFQLineFormTypeService rfqLineFormTypeService,
                                             IEquipmentPurchaseTypeService equipmentPurchaseTypeService,
                                             IEquipmentCalibrationTypeService equipmentCalibrationTypeService,
                                             bool userIsBuyer,
                                             bool userIsRequester)
        {
            
            var indx = 0;

            foreach (var attachmentVm in model.Attachments)
            {
                attachmentVm.Index = indx.ToString();
                indx++;

                if (rfqLine.RFQ.RFQStatusId == (int)RFQStates.Submitted)
                    attachmentVm.RFQIsSubmitted = true;
                else
                    attachmentVm.RFQIsSubmitted = false;
            }

            model.PopulateLookupsForLineModel(uomService, rfqLineTypeService, rfqLineFormTypeService, equipmentPurchaseTypeService, equipmentCalibrationTypeService, currencyService);

            model.RFQIsSubmitted = rfqLine.RFQ.RFQStatusId == (int)RFQStates.Submitted ? true : false;
            model.UserIsBuyer = userIsBuyer;
            model.UserIsRequestor = userIsRequester;
            model.UserCanAddDeleteQuotation = userIsBuyer;

            indx = 0;

            model.Quotations = model.Quotations.Where(x => x.IsDeleted == false).ToList();

            foreach (var quotationVm in model.Quotations)
            {
                quotationVm.Index = indx++.ToString();
                quotationVm.RFQLineId = rfqLine.Id;
                quotationVm.PopulateLookupsForQuotationModel(currencyService);
                quotationVm.UserIsBuyer = userIsBuyer;
                quotationVm.UserIsRequestor = userIsRequester;

                var quoteIndx = 0;

                quotationVm.QuoteAttachment = quotationVm.QuoteAttachment.Where(x => x.IsDeleted == false).ToList();
                foreach (var quoteAttachment in quotationVm.QuoteAttachment)
                {
                    quoteAttachment.Index = quoteIndx++.ToString();
                    quoteAttachment.QuotationIndex = quotationVm.Index;
                    quoteAttachment.RFQLineId = rfqLine.Id;

                    if (rfqLine.RFQ.RFQStatusId == (int)RFQStates.Submitted)
                        quoteAttachment.RFQIsSubmitted = true;
                    else
                        quoteAttachment.RFQIsSubmitted = false;
                }

                quoteIndx = 0;
                quotationVm.ROHSDocumentAttachment = quotationVm.ROHSDocumentAttachment.Where(x => x.IsDeleted == false).ToList();
                foreach (var quoteAttachment in quotationVm.ROHSDocumentAttachment)
                {
                    quoteAttachment.Index = quoteIndx++.ToString();
                    quoteAttachment.QuotationIndex = quotationVm.Index;
                    quoteAttachment.RFQLineId = rfqLine.Id;

                    if (rfqLine.RFQ.RFQStatusId == (int)RFQStates.Submitted)
                        quoteAttachment.RFQIsSubmitted = true;
                    else
                        quoteAttachment.RFQIsSubmitted = false;
                }

                quoteIndx = 0;
                quotationVm.MSDSDocumentAttachment = quotationVm.MSDSDocumentAttachment.Where(x => x.IsDeleted == false).ToList();
                foreach (var quoteAttachment in quotationVm.MSDSDocumentAttachment)
                {
                    quoteAttachment.Index = quoteIndx++.ToString();
                    quoteAttachment.QuotationIndex = quotationVm.Index;
                    quoteAttachment.RFQLineId = rfqLine.Id;

                    if (rfqLine.RFQ.RFQStatusId == (int)RFQStates.Submitted)
                        quoteAttachment.RFQIsSubmitted = true;
                    else
                        quoteAttachment.RFQIsSubmitted = false;
                }

                quoteIndx = 0;
                quotationVm.OtherAttachments = quotationVm.OtherAttachments.Where(x => x.IsDeleted == false).ToList();
                foreach (var quoteAttachment in quotationVm.OtherAttachments)
                {
                    quoteAttachment.Index = quoteIndx++.ToString();
                    quoteAttachment.QuotationIndex = quotationVm.Index;
                    quoteAttachment.RFQLineId = rfqLine.Id;

                    if (rfqLine.RFQ.RFQStatusId == (int)RFQStates.Submitted)
                        quoteAttachment.RFQIsSubmitted = true;
                    else
                        quoteAttachment.RFQIsSubmitted = false;
                }
            }

            //Check if parent RFQ is submitted...
            if (rfqLine.RFQ.RFQStatusId == (int)RFQStates.Submitted)
                model.RFQIsSubmitted = true;
            else
                model.RFQIsSubmitted = false;
        }

        public static void ToEntityForAdding(this RFQLine rfqLine, RFQLineModel model, IRFQLineStatusService rfqLineStatusService, int currentUserId)
        {
            if (model.Attachments == null)
                model.Attachments = new List<FileAttachmentModel>();

            model.Attachments = model.Attachments.Where(x => x.IsDeleted == false && !String.IsNullOrWhiteSpace(x.Filename)).ToList();

            rfqLine.Id = model.Id;
            rfqLine.RFQId = model.RFQId;
            rfqLine.RFQLineNo = model.RFQLineNo;
            rfqLine.ItemDescription = model.ItemDescription;
            rfqLine.Quantity = model.Quantity;
            rfqLine.Maker = model.Maker;
            rfqLine.MakerPN = model.MakerPN;
            rfqLine.ReferenceLinks = model.ReferenceLinks;
            rfqLine.ROHSCompliant = model.ROHSCompliant;
            rfqLine.OtherTechnicalDetails = model.OtherTechnicalDetails;
            rfqLine.NoteToBuyer = model.NoteToBuyer;
            rfqLine.TestEquipmentApplication = model.TestEquipmentApplication;
            rfqLine.LineTypeId = model.LineTypeId;
            rfqLine.FormTypeId = model.FormTypeId;
            rfqLine.TestEquipmentPurchaseTypeId = model.TestEquipmentPurchaseTypeId;
            rfqLine.TestEquipmentCalibrationTypeId = model.TestEquipmentCalibrationTypeId;
            rfqLine.QuantityUnitId = model.QuantityUnitId;

            if (rfqLine.Attachments == null)
                rfqLine.Attachments = new List<FileAttachment>();

            foreach (var attachment in model.Attachments)
            {
                var newAttachment = new FileAttachment()
                {
                    Id = attachment.Id,
                    Filename = attachment.Filename,
                    FileUrl = attachment.FileUrl,
                    FileAttachmentType = (int)FileAttachmentType.RFQLineAttachment,
                    Active = true
                };
                newAttachment.PopulateAuditFieldsForSave(currentUserId);
                
                rfqLine.Attachments.Add(newAttachment);
            }

            rfqLine.PopulateAuditFieldsForSave(currentUserId);
            rfqLine.RFQLineStatusId = (int)RFQLineStates.Open;
            rfqLine.Active = true;

        }

        public static void ToEntityForEditing(this RFQLine existingRFQLine, RFQLineModel model, IFileAttachmentService fileAttachmentService, int currentUserId, string emailAttachmentFilePath, string editUrl, string quotationAttachmentDir, string rohsAttachmentDir, string MSDSAttachmentDir, string otherAttachmentDir)
        {
            //Map view model to domain model...
            existingRFQLine.ItemDescription = model.ItemDescription;
            existingRFQLine.Quantity = model.Quantity;
            existingRFQLine.Maker = model.Maker;
            existingRFQLine.MakerPN = model.MakerPN;
            existingRFQLine.ReferenceLinks = model.ReferenceLinks;
            existingRFQLine.ROHSCompliant = model.ROHSCompliant;
            existingRFQLine.OtherTechnicalDetails = model.OtherTechnicalDetails;
            existingRFQLine.NoteToBuyer = model.NoteToBuyer;
            existingRFQLine.TestEquipmentApplication = model.TestEquipmentApplication;
            existingRFQLine.FirstUploadDate = model.FirstUploadDate;
            existingRFQLine.LineTypeId = model.LineTypeId;
            existingRFQLine.FormTypeId = model.FormTypeId;
            existingRFQLine.TestEquipmentPurchaseTypeId = model.TestEquipmentPurchaseTypeId;
            existingRFQLine.TestEquipmentCalibrationTypeId = model.TestEquipmentCalibrationTypeId;
            existingRFQLine.QuantityUnitId = model.QuantityUnitId;

            existingRFQLine.PopulateAuditFieldsForEdit(currentUserId);

            existingRFQLine.RFQ.SetEmailAttachmentFilePath(emailAttachmentFilePath);
            existingRFQLine.RFQ.EditUrl = editUrl;

            //Attachments...
            existingRFQLine.MapLineAttachments(model, fileAttachmentService, currentUserId);

            if (existingRFQLine.NewQuotationsForUpload == null)
                existingRFQLine.NewQuotationsForUpload = new List<int>();

            //Quotations...
            MapQuotations(existingRFQLine, model, fileAttachmentService, currentUserId, quotationAttachmentDir, rohsAttachmentDir, MSDSAttachmentDir, otherAttachmentDir);
        }

        public static void MapQuotationAttachments(this Quotation existingQuotation, QuotationModel model, IFileAttachmentService fileAttachmentService, int currentUserId, FileAttachmentType attachmentType)
        {
            var modelAttachments = model.GetAttachmentsByType(attachmentType);

            foreach (var attachment in modelAttachments)
            {
                if (attachment.Id > 0)
                {
                    if (attachment.IsDeleted == true || String.IsNullOrWhiteSpace(attachment.Filename))
                    {
                        var existingAttachment = fileAttachmentService.GetFileAttachmentById(attachment.Id);

                        if (existingAttachment != null)
                        {
                            existingQuotation.RemoveAttachment(existingAttachment, attachmentType);
                        }
                    }
                    else
                    {
                        FileAttachment existingAttachment = existingQuotation.GetAttachmentById(attachment.Id, attachmentType);

                        if (existingAttachment != null)
                        {
                            existingAttachment.Filename = attachment.Filename;
                            existingAttachment.FileUrl = attachment.FileUrl;
                            existingAttachment.FileAttachmentType = (int)attachmentType;
                            existingAttachment.Active = true;
                            existingAttachment.PopulateAuditFieldsForEdit(currentUserId);
                        }
                    }
                }
                else
                {
                    if (attachment.IsDeleted == false && !String.IsNullOrWhiteSpace(attachment.Filename))
                    {
                        var newAttachment = new FileAttachment();
                        newAttachment.Filename = attachment.Filename;
                        newAttachment.FileUrl = attachment.FileUrl;
                        newAttachment.FileAttachmentType = (int)attachmentType;
                        newAttachment.PopulateAuditFieldsForSave(currentUserId);
                        newAttachment.Active = true;

                        existingQuotation.AddAttachment(newAttachment, attachmentType);
                    }
                }
            }
        }

        public static QuotationModel ToEntity(this Quotation quotation)
        {
            var model = new QuotationModel();

            model.Id = quotation.Id;
            model.QuotationNo = quotation.QuotationNo;
            model.QuoteUploadDate = quotation.QuoteUploadDate;
            model.Commodity = quotation.Commodity;
            model.QuoteValidity = quotation.QuoteValidity;
            model.QuoteItemDescription = quotation.QuoteItemDescription;
            model.MakerPN = quotation.MakerPN;
            model.UnitPrice = quotation.UnitPrice;
            model.DiscountedPrice = quotation.DiscountedPrice;
            model.MinimumOrderQuantity = quotation.MinimumOrderQuantity;
            model.PaymentTerms = quotation.PaymentTerms;
            model.Supplier = quotation.Supplier;
            model.DeliveryLeadTime = quotation.DeliveryLeadTime;
            model.WarrantyPeriod = quotation.WarrantyPeriod;
            model.BuyerRemarks = quotation.BuyerRemarks;
            model.RequesterRemarks = quotation.RequesterRemarks;
            model.RFQLineId = quotation.RFQLineId;
            model.Active = quotation.Active;
            model.IsDeleted = quotation.Active == true ? false : true;

            if (quotation.Currency != null)
            {
                model.Currency = quotation.Currency.CurrencyCode;
                model.CurrencyId = quotation.Currency.Id;
            }
            
            //Quote attachments...
            model.QuoteAttachment = new List<FileAttachmentModel>();

            foreach (var attachment in quotation.QuoteAttachment)
            {
                model.QuoteAttachment.Add(new FileAttachmentModel() { Filename = attachment.Filename, FileUrl = attachment.FileUrl, FileAttachmentType = attachment.FileAttachmentType });
            }

            //ROHS attachments...
            model.ROHSDocumentAttachment = new List<FileAttachmentModel>();

            foreach (var attachment in quotation.ROHSDocumentAttachment)
            {
                model.ROHSDocumentAttachment.Add(new FileAttachmentModel() { Filename = attachment.Filename, FileUrl = attachment.FileUrl, FileAttachmentType = attachment.FileAttachmentType });
            }

            //MSDS Attachments...
            model.MSDSDocumentAttachment = new List<FileAttachmentModel>();

            foreach (var attachment in quotation.MSDSDocumentAttachment)
            {
                model.MSDSDocumentAttachment.Add(new FileAttachmentModel() { Filename = attachment.Filename, FileUrl = attachment.FileUrl, FileAttachmentType = attachment.FileAttachmentType });
            }

            //Other Attachments...
            model.OtherAttachments = new List<FileAttachmentModel>();

            foreach (var attachment in quotation.OtherAttachments)
            {
                model.OtherAttachments.Add(new FileAttachmentModel() { Filename = attachment.Filename, FileUrl = attachment.FileUrl, FileAttachmentType = attachment.FileAttachmentType });
            }

            return model;
        }

        public static void MapQuotations(this RFQLine existingRFQLine, RFQLineModel model, IFileAttachmentService fileAttachmentService, int currentUserId, string quotationAttachmentDir, string rohsAttachmentDir, string MSDSAttachmentDir, string otherAttachmentDir)
        {
            //Save attachments directories to be used later for email notification...
            existingRFQLine.QuotationAttachmentDir = quotationAttachmentDir;
            existingRFQLine.ROHSAttachmentDir = rohsAttachmentDir;
            existingRFQLine.MSDSAttachmentDir = MSDSAttachmentDir;
            existingRFQLine.OtherAttachmentDir = otherAttachmentDir;

            //Check new quotation has been uploaded...
            //Do this before mapping...otherwise we can't determine if a new quotation has been added...
            existingRFQLine.CheckIfFirstQuotationUpload(model);

            //Remember this new quotation later for email notification...
            if (existingRFQLine.NewQuotationsForUpload == null)
                existingRFQLine.NewQuotationsForUpload = new List<int>();

            foreach (var quotation in model.Quotations)
            {
                if (quotation.Id > 0)
                {
                    var quotationToEdit = existingRFQLine.Quotations.Where(x => x.Id == quotation.Id).FirstOrDefault();
                    
                    if (quotation.IsDeleted == true)
                    {
                        if (quotationToEdit != null)
                        {
                            quotationToEdit.Active = false;
                            quotationToEdit.PopulateAuditFieldsForEdit(currentUserId);
                        }
                    }
                    else
                    {
                        if (quotationToEdit != null)
                        {
                            quotationToEdit.Commodity = quotation.Commodity;
                            quotationToEdit.QuoteValidity = quotation.QuoteValidity;
                            quotationToEdit.QuoteItemDescription = quotation.QuoteItemDescription;
                            quotationToEdit.MakerPN = quotation.MakerPN;
                            quotationToEdit.UnitPrice = quotation.UnitPrice;
                            quotationToEdit.DiscountedPrice = quotation.DiscountedPrice;
                            quotationToEdit.MinimumOrderQuantity = quotation.MinimumOrderQuantity;
                            quotationToEdit.PaymentTerms = quotation.PaymentTerms;
                            quotationToEdit.Supplier = quotation.Supplier;
                            quotationToEdit.DeliveryLeadTime = quotation.DeliveryLeadTime;
                            quotationToEdit.WarrantyPeriod = quotation.WarrantyPeriod;
                            quotationToEdit.BuyerRemarks = quotation.BuyerRemarks;
                            quotationToEdit.RequesterRemarks = quotation.RequesterRemarks;
                            quotationToEdit.CurrencyId = quotation.CurrencyId;
                            quotationToEdit.PopulateAuditFieldsForEdit(currentUserId);

                            quotationToEdit.MapQuotationAttachments(quotation, fileAttachmentService, currentUserId, FileAttachmentType.QuoteAttachment);
                            quotationToEdit.MapQuotationAttachments(quotation, fileAttachmentService, currentUserId, FileAttachmentType.ROHSDocumentAttachment);
                            quotationToEdit.MapQuotationAttachments(quotation, fileAttachmentService, currentUserId, FileAttachmentType.MSDSDocumentAttachment);
                            quotationToEdit.MapQuotationAttachments(quotation, fileAttachmentService, currentUserId, FileAttachmentType.OtherAttachmentDrawing);

                        }
                    }
                }
                else
                {
                    if (quotation.IsDeleted == false)
                    {
                        var newQuotation = new Quotation();
                        newQuotation.RFQLineId = existingRFQLine.Id;
                        newQuotation.QuoteUploadDate = DateTime.UtcNow;
                        newQuotation.RFQLineId = existingRFQLine.Id;
                        newQuotation.QuotationNo = quotation.QuotationNo;
                        newQuotation.Commodity = quotation.Commodity;
                        newQuotation.QuoteValidity = quotation.QuoteValidity;
                        newQuotation.QuoteItemDescription = quotation.QuoteItemDescription;
                        newQuotation.MakerPN = quotation.MakerPN;
                        newQuotation.UnitPrice = quotation.UnitPrice;
                        newQuotation.DiscountedPrice = quotation.DiscountedPrice;
                        newQuotation.MinimumOrderQuantity = quotation.MinimumOrderQuantity;
                        newQuotation.PaymentTerms = quotation.PaymentTerms;
                        newQuotation.Supplier = quotation.Supplier;
                        newQuotation.DeliveryLeadTime = quotation.DeliveryLeadTime;
                        newQuotation.WarrantyPeriod = quotation.WarrantyPeriod;
                        newQuotation.BuyerRemarks = quotation.BuyerRemarks;
                        newQuotation.RequesterRemarks = quotation.RequesterRemarks;
                        newQuotation.CurrencyId = quotation.CurrencyId;
                        newQuotation.PopulateAuditFieldsForSave(currentUserId);
                        newQuotation.Active = true;

                        newQuotation.MapQuotationAttachments(quotation, fileAttachmentService, currentUserId, FileAttachmentType.QuoteAttachment);
                        newQuotation.MapQuotationAttachments(quotation, fileAttachmentService, currentUserId, FileAttachmentType.ROHSDocumentAttachment);
                        newQuotation.MapQuotationAttachments(quotation, fileAttachmentService, currentUserId, FileAttachmentType.MSDSDocumentAttachment);
                        newQuotation.MapQuotationAttachments(quotation, fileAttachmentService, currentUserId, FileAttachmentType.OtherAttachmentDrawing);

                        existingRFQLine.Quotations.Add(newQuotation);
                        existingRFQLine.NewQuotationsForUpload.Add(newQuotation.QuotationNo);
                    }

                }
            }
        }

        public static void MapLineAttachments(this RFQLine existingRFQLine, RFQLineModel model, IFileAttachmentService fileAttachmentService, int currentUserId)
        {
            if (model.Attachments == null)
                model.Attachments = new List<FileAttachmentModel>();

            foreach (var attachment in model.Attachments)
            {
                if (attachment.Id > 0)
                {
                    if (attachment.IsDeleted == true || String.IsNullOrWhiteSpace(attachment.Filename))
                    {
                        var existingAttachment = fileAttachmentService.GetFileAttachmentById(attachment.Id);

                        if (existingAttachment != null)
                        {
                            existingRFQLine.Attachments.Remove(existingAttachment);
                        }
                    }
                    else
                    {
                        var existingAttachment = existingRFQLine.Attachments.Where(x => x.Id == attachment.Id).FirstOrDefault();

                        if (existingAttachment != null)
                        {
                            existingAttachment.Filename = attachment.Filename;
                            existingAttachment.FileUrl = attachment.FileUrl;
                            existingAttachment.FileAttachmentType = (int)FileAttachmentType.RFQLineAttachment;
                            existingAttachment.Active = true;
                            existingAttachment.PopulateAuditFieldsForEdit(currentUserId);
                        }
                    }
                }
                else
                {
                    if (attachment.IsDeleted == false && !String.IsNullOrWhiteSpace(attachment.Filename))
                    {
                        var newAttachment = new FileAttachment();
                        newAttachment.Filename = attachment.Filename;
                        newAttachment.FileUrl = attachment.FileUrl;
                        newAttachment.FileAttachmentType = attachment.FileAttachmentType;
                        newAttachment.PopulateAuditFieldsForSave(currentUserId);
                        newAttachment.Active = true;

                        existingRFQLine.Attachments.Add(newAttachment);
                    }
                }
            }
        }

        #endregion
    }
}