using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FluentValidation.Attributes;
using SSG.Web.Validators.RFQ;
using System.Web.Mvc;
using SSG.Core.Domain.RFQ;

namespace SSG.Web.Models.RFQ
{
    [Validator(typeof(QuotationValidator))]
    public class QuotationModel
    {
        public QuotationModel()
        {
            AvailableCurrencies = new List<SelectListItem>();
            QuoteAttachment = new List<FileAttachmentModel>();
            ROHSDocumentAttachment = new List<FileAttachmentModel>();
            MSDSDocumentAttachment = new List<FileAttachmentModel>();
            OtherAttachments = new List<FileAttachmentModel>();
        }

        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("Quotation No.")]
        public int QuotationNo { get; set; }

        [DisplayName("Quote Upload Date")]
        public DateTime? QuoteUploadDate { get; set; }

        [DisplayName("Commodity")]
        public string Commodity { get; set; }

        [DisplayName("Quote Validity")]
        public string QuoteValidity { get; set; }

        [DisplayName("Quote Item Description")]
        public string QuoteItemDescription { get; set; }

        [DisplayName("MakerPN")]
        public string MakerPN { get; set; }

        [DisplayName("Unit Price")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }

        [DisplayName("Discounted Price")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal DiscountedPrice { get; set; }

        [DisplayName("Minimum Order Quantity (MOQ)")]
        public int MinimumOrderQuantity { get; set; }

        [DisplayName("Payment Terms")]
        public string PaymentTerms { get; set; }


        [DisplayName("Supplier")]
        public string Supplier { get; set; }

        [DisplayName("Delivery Lead Time")]
        public string DeliveryLeadTime { get; set; }

        [DisplayName("WarrantyPeriod")]
        public string WarrantyPeriod { get; set; }

        [DisplayName("Buyer Remarks")]
        public string BuyerRemarks { get; set; }

        [DisplayName("Requester Remarks")]
        public string RequesterRemarks { get; set; }

        [DisplayName("Quote Attachment")]
        public List<FileAttachmentModel> QuoteAttachment { get; set; }

        [DisplayName("ROHS Document")]
        public List<FileAttachmentModel> ROHSDocumentAttachment { get; set; }

        [DisplayName("MSDS Attachment")]
        public List<FileAttachmentModel> MSDSDocumentAttachment { get; set; }

        [DisplayName("Other Attachment: (Drawing)")]
        public List<FileAttachmentModel> OtherAttachments { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }

        #region "Currency"

        [DisplayName("Currency")]
        [AllowHtml]
        public int CurrencyId { get; set; }
        public string Currency { get; set; }
        public List<SelectListItem> AvailableCurrencies { get; set; }

        #endregion

        public int RFQLineId { get; set; }

        public string Index { get; set; }

        public bool IsDeleted { get; set; }

        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime DateCreatedOnUtc { get; set; }
        public DateTime DateUpdatedOnUtc { get; set; }

        #region helper methods

        public bool UserIsBuyer { get; set; }

        public bool UserIsRequestor { get; set; }

        private void InitializeAttachments()
        {
            if (this.QuoteAttachment == null)
                QuoteAttachment = new List<FileAttachmentModel>();

            if (this.ROHSDocumentAttachment == null)
                ROHSDocumentAttachment = new List<FileAttachmentModel>();

            if (this.MSDSDocumentAttachment == null)
                MSDSDocumentAttachment = new List<FileAttachmentModel>();

            if (this.OtherAttachments ==  null)
                OtherAttachments = new List<FileAttachmentModel>();
        }

        public List<FileAttachmentModel> GetAttachmentsByType(FileAttachmentType attachmentType)
        {
            InitializeAttachments();

            var modelAttachments = new List<FileAttachmentModel>();

            if (attachmentType == FileAttachmentType.QuoteAttachment)
                modelAttachments = this.QuoteAttachment;
            else if (attachmentType == FileAttachmentType.ROHSDocumentAttachment)
                modelAttachments = this.ROHSDocumentAttachment;
            else if (attachmentType == FileAttachmentType.MSDSDocumentAttachment)
                modelAttachments = this.MSDSDocumentAttachment;
            else
                modelAttachments = this.OtherAttachments;

            return modelAttachments;
        }

        #endregion
    }
}