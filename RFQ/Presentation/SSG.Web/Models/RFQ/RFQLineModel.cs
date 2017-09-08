﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SSG.Web.Models.FineUploader;
using System.Web.Mvc;
using FluentValidation.Attributes;
using PagedList;
using SSG.Web.Validators.RFQ;
using SSG.Web.Controllers;

namespace SSG.Web.Models.RFQ
{
    [Validator(typeof(RFQLineValidator))]
    public partial class RFQLineModel
    {
        public RFQLineModel()
        {
            AvailableUnits = new List<SelectListItem>();
            AvailableLineTypes = new List<SelectListItem>();
            AvailableFormTypes = new List<SelectListItem>();
            AvailableEquipmentPurchaseTypes = new List<SelectListItem>();
            AvailableEquipmentCalibrationTypes = new List<SelectListItem>();
            StatusSelectListItems = new List<SelectListItem>();
            Quotations = new List<QuotationModel>();
        }

        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("RFQ No.")]
        public string RFQNo { get; set; }

        [DisplayName("RFQ Creation Date")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime RFQCreationDate { get; set; }

        [DisplayName("RFQ Status")]
        public string RFQStatus { get; set; }

        [DisplayName("RFQ Id")]
        public int RFQId { get; set; }

        [DisplayName("RFQ Line No")]
        public int RFQLineNo { get; set; }

        [DisplayName("Item Description")]
        [AllowHtml]
        public string ItemDescription { get; set; }

        [DisplayName("Quantity")]
        public int Quantity { get; set; }

        [DisplayName("UOM")]
        [AllowHtml]
        public int QuantityUnitId { get; set; }
        public string QuantityUnit { get; set; }
        public List<SelectListItem> AvailableUnits { get; set; }

        [DisplayName("Maker")]
        [AllowHtml]
        public string Maker { get; set; }

        [DisplayName("MakerPN")]
        [AllowHtml]
        public string MakerPN { get; set; }

        [DisplayName("Purpose of the Request")]
        public string RequestPurpose { get; set; }

        [DisplayName("Reference Links")]
        public string ReferenceLinks { get; set; }

        [DisplayName("ROHS Compliant")]
        public bool ROHSCompliant { get; set; }

        [DisplayName("Other Technical Details")]
        public string OtherTechnicalDetails { get; set; }

        [DisplayName("Note to Buyer")]
        public string NoteToBuyer { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        [DisplayName("ClosedDate")]
        public DateTime? FirstUploadDate { get; set; }

        [DisplayName("Attachments")]
        public IList<FileAttachmentModel> Attachments { get; set; }
        
        [DisplayName("Test Equipment Purchase Type")]
        [AllowHtml]
        public int? TestEquipmentPurchaseTypeId { get; set; }
        public string TestEquipmentPurchaseType { get; set; }
        public IList<SelectListItem> AvailableEquipmentPurchaseTypes { get; set; }

        [DisplayName("Test Equipment Calibration Type")]
        [AllowHtml]
        public int? TestEquipmentCalibrationTypeId { get; set; }
        public string TestEquipmentCalibrationType { get; set; }
        public IList<SelectListItem> AvailableEquipmentCalibrationTypes { get; set; }

        [DisplayName("Test Equipment Application")]
        public string TestEquipmentApplication { get; set; }
        
        [DisplayName("Active")]
        public bool Active { get; set; }

        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DisplayName("RFQLine Creation Date")]
        public DateTime DateCreatedOnUtc { get; set; }
        public DateTime DateUpdatedOnUtc { get; set; }

        //References
        [DisplayName("RFQ Line Type")]
        [AllowHtml]
        public int LineTypeId { get; set; }
        public string LineType { get; set; }
        public IList<SelectListItem> AvailableLineTypes { get; set; }

        [DisplayName("RFQ Line Form Type")]
        [AllowHtml]
        public int FormTypeId { get; set; }
        public string FormType { get; set; }
        public IList<SelectListItem> AvailableFormTypes { get; set; }

        [DisplayName("Status")]
        [AllowHtml]
        public int StatusId { get; set; }
        public string Status { get; set; }
        public IList<SelectListItem> StatusSelectListItems { get; set; }

        public List<QuotationModel> Quotations { get; set; }

        public bool RFQIsSubmitted { get; set; }

        public bool IsUserCanEditable { get; set; }

        public bool IsUserCanDeletable { get; set; }

        public bool UserCanAddDeleteQuotation { get; set; }

        public bool UserIsBuyer { get; set; }

        public bool UserIsRequestor { get; set; }


        #region "Helper Methods"

        public List<string> GetReferencesForDisplay()
        {
            char delimiter = ';';
            var referencesList = new List<string>();

            if (this.ReferenceLinks != null)
            {
                referencesList = this.ReferenceLinks.Split(delimiter).ToList();
            }

            return referencesList;
        }

        #endregion
    }
}