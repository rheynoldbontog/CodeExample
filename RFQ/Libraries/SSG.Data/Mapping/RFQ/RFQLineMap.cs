﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

namespace SSG.Data.Mapping.RFQ
{
    public class RFQLineMap : EntityTypeConfiguration<SSG.Core.Domain.RFQ.RFQLine>
    {
        public RFQLineMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.RFQLineNo).IsRequired().HasColumnName("RFQLineNo");
            this.Property(t => t.ItemDescription).IsRequired().HasMaxLength(100).HasColumnName("ItemDescription");
            this.Property(t => t.Quantity).IsRequired().HasColumnName("Quantity");
            this.Property(t => t.Maker).IsOptional().HasMaxLength(50).HasColumnName("Maker");
            this.Property(t => t.MakerPN).IsOptional().HasMaxLength(50).HasColumnName("MakerPN");
            this.Property(t => t.ReferenceLinks).IsOptional().HasMaxLength(1000).HasColumnName("LinksAndReferences");
            this.Property(t => t.ROHSCompliant).HasColumnName("ROHSCompliant");
            this.Property(t => t.OtherTechnicalDetails).IsOptional().HasMaxLength(300).HasColumnName("OtherTechnicalDetails");
            this.Property(t => t.NoteToBuyer).IsOptional().HasMaxLength(250).HasColumnName("NoteToBuyer");
            this.Property(t => t.TestEquipmentApplication).IsOptional().HasMaxLength(100).HasColumnName("TestEquipmentApplication");
            this.Ignore(t => t.HasFirstUpload);
            this.Ignore(t => t.NewQuotationsForUpload);
            this.Ignore(t => t.QuotationAttachmentDir);
            this.Ignore(t => t.ROHSAttachmentDir);
            this.Ignore(t => t.MSDSAttachmentDir);
            this.Ignore(t => t.OtherAttachmentDir);

            // References
            this.HasRequired(t => t.RFQ).WithMany(t => t.Lines).HasForeignKey(t => t.RFQId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.Status).WithMany(x => x.Lines).HasForeignKey(x => x.RFQLineStatusId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.QuantityUnit).WithMany(t => t.Lines).HasForeignKey(t => t.QuantityUnitId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.LineType).WithMany(t => t.Lines).HasForeignKey(t => t.LineTypeId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.FormType).WithMany(f => f.Lines).HasForeignKey(t => t.FormTypeId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.TestEquipmentPurchaseType).WithMany(t => t.RFQLines).HasForeignKey(t => t.TestEquipmentPurchaseTypeId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.TestEquipmentCalibrationType).WithMany(t => t.RFQLines).HasForeignKey(t => t.TestEquipmentCalibrationTypeId).WillCascadeOnDelete(false);
            this.HasMany(t => t.Attachments).WithOptional().WillCascadeOnDelete(false);
            this.HasMany(x => x.StatusHistory).WithRequired(x => x.RFQLine).HasForeignKey(x => x.RFQLineId).WillCascadeOnDelete(false);

            // Table & Column Mappings
            this.ToTable("rfqline", "rfq");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.DateCreatedOnUtc).HasColumnName("DateCreatedOnUtc");
            this.Property(t => t.DateUpdatedOnUtc).HasColumnName("DateUpdatedOnUtc");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
        }
    }
}