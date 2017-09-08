using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.Directory;

namespace SSG.Core.Domain.RFQ
{
    public partial class RFQLine : BaseAuditEntity
    {
        public int RFQLineNo { get; set; }

        public string ItemDescription { get; set; }

        public int Quantity { get; set; }

        public string Maker { get; set; }

        public string MakerPN { get; set; }
        
        public string ReferenceLinks { get; set; }

        public bool ROHSCompliant { get; set; }

        public string OtherTechnicalDetails { get; set; }

        public string NoteToBuyer { get; set; }

        public string TestEquipmentApplication { get; set; }

        public bool Active { get; set; }

        public DateTime? FirstUploadDate { get; set; }

        //REFERENCES
        #region RFQ

        public virtual int RFQId { get; set; }

        public virtual RFQ RFQ { get; set; }

        #endregion

        #region "Line Type"

        public virtual int LineTypeId { get; set; }

        public virtual RFQLineType LineType { get; set; }

        #endregion

        #region "Form Type"

        public virtual int FormTypeId { get; set; }

        public virtual RFQLineFormType FormType { get; set; }

        #endregion

        #region Test Equipment Purchase Type

        public virtual int? TestEquipmentPurchaseTypeId { get; set; }

        public virtual EquipmentPurchaseType TestEquipmentPurchaseType { get; set; }

        #endregion

        #region Test Equipment Calibration Type

        public virtual int? TestEquipmentCalibrationTypeId { get; set; }

        public virtual EquipmentCalibrationType TestEquipmentCalibrationType { get; set; }

        #endregion

        #region "Quantity Unit"

        public virtual int QuantityUnitId { get; set; }
        public virtual UOM QuantityUnit { get; set; }

        #endregion

        #region Quotations

        public virtual ICollection<Quotation> Quotations { get; set; }

        #endregion

        #region "File Attachments"

        public virtual ICollection<FileAttachment> Attachments { get; set; }

        #endregion

        #region "Status"

        public virtual int RFQLineStatusId { get; set; }

        public virtual RFQLineStatus Status { get; set; }

        #endregion

        #region "Status History"

        public virtual ICollection<RFQLineStatusHistory> StatusHistory { get; set; }

        #endregion

        #region "Helper Properties"
        //These are for keeping temporary data only...
        //They are not mapped to the database...
        public virtual bool HasFirstUpload { get; set; }

        public virtual List<int> NewQuotationsForUpload { get; set; }

        public virtual string QuotationAttachmentDir { get; set;}

        public virtual string ROHSAttachmentDir { get; set; }

        public virtual string MSDSAttachmentDir { get; set; }

        public virtual string OtherAttachmentDir { get; set; }

        #endregion
    }
}
