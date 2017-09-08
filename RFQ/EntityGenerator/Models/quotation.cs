using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class quotation
    {
        public int Id { get; set; }
        public string QuotationNo { get; set; }
        public Nullable<System.DateTime> QuoteUploadDate { get; set; }
        public string QuoteItemDescription { get; set; }
        public string MakerPN { get; set; }
        public decimal UnitPrice { get; set; }
        public Nullable<decimal> DiscountedPrice { get; set; }
        public Nullable<int> MinimumOrderQuantity { get; set; }
        public string PaymentTerms { get; set; }
        public string Supplier { get; set; }
        public string DeliveryLeadTime { get; set; }
        public string WarrantyPeriod { get; set; }
        public string BuyerRemarks { get; set; }
        public string RequesterRemarks { get; set; }
        public string QuotationAttachment { get; set; }
        public string ROHSDocument { get; set; }
        public string MSDSAttachment { get; set; }
        public string OtherAttachment1 { get; set; }
        public string OtherAttachment2 { get; set; }
        public string OtherAttachment3 { get; set; }
        public string OtherAttachment4 { get; set; }
        public string OtherAttachment5 { get; set; }
        public bool Active { get; set; }
        public int RFQLineId { get; set; }
        public int CurrencyId { get; set; }
        public System.DateTime DateCreatedOnUtc { get; set; }
        public System.DateTime DateUpdatedOnUtc { get; set; }
        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public virtual currency currency { get; set; }
        public virtual user user { get; set; }
        public virtual rfqline rfqline { get; set; }
        public virtual user user1 { get; set; }
    }
}
