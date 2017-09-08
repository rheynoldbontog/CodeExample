using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class rfqline
    {
        public rfqline()
        {
            this.quotations = new List<quotation>();
        }

        public int Id { get; set; }
        public int RFQLineNo { get; set; }
        public string ItemDescription { get; set; }
        public int Quantity { get; set; }
        public string Maker { get; set; }
        public string MakerPN { get; set; }
        public string RequestPurpose { get; set; }
        public string LinksAndReferences { get; set; }
        public bool ROHSCompliant { get; set; }
        public string OtherTechnicalDetails { get; set; }
        public string NoteToBuyer { get; set; }
        public string Attachment1 { get; set; }
        public string Attachment2 { get; set; }
        public string Attachment3 { get; set; }
        public string TestEquipmentApplication { get; set; }
        public Nullable<System.DateTime> RentalPeriodFrom { get; set; }
        public Nullable<System.DateTime> RentalPeriodTo { get; set; }
        public bool Active { get; set; }
        public int RFQId { get; set; }
        public int LineTypeId { get; set; }
        public int FormTypeId { get; set; }
        public Nullable<int> TestEquipmentPurchaseTypeId { get; set; }
        public Nullable<int> TestEquipmentCalibrationTypeId { get; set; }
        public int CurrencyId { get; set; }
        public int QuantityUnitId { get; set; }
        public System.DateTime DateCreatedOnUtc { get; set; }
        public System.DateTime DateUpdatedOnUtc { get; set; }
        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public virtual currency currency { get; set; }
        public virtual equipmentcalibrationtype equipmentcalibrationtype { get; set; }
        public virtual equipmentpurchasetype equipmentpurchasetype { get; set; }
        public virtual ICollection<quotation> quotations { get; set; }
        public virtual rfq rfq { get; set; }
        public virtual user user { get; set; }
        public virtual rfqlinetype rfqlinetype { get; set; }
        public virtual user user1 { get; set; }
        public virtual rfqlineformtype rfqlineformtype { get; set; }
        public virtual uom uom { get; set; }
    }
}
