using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class rfqlineMap : EntityTypeConfiguration<rfqline>
    {
        public rfqlineMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ItemDescription)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Maker)
                .HasMaxLength(50);

            this.Property(t => t.MakerPN)
                .HasMaxLength(50);

            this.Property(t => t.RequestPurpose)
                .HasMaxLength(200);

            this.Property(t => t.LinksAndReferences)
                .HasMaxLength(1000);

            this.Property(t => t.OtherTechnicalDetails)
                .HasMaxLength(300);

            this.Property(t => t.NoteToBuyer)
                .HasMaxLength(100);

            this.Property(t => t.Attachment1)
                .HasMaxLength(100);

            this.Property(t => t.Attachment2)
                .HasMaxLength(100);

            this.Property(t => t.Attachment3)
                .HasMaxLength(100);

            this.Property(t => t.TestEquipmentApplication)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("rfqline", "rfq");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RFQLineNo).HasColumnName("RFQLineNo");
            this.Property(t => t.ItemDescription).HasColumnName("ItemDescription");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Maker).HasColumnName("Maker");
            this.Property(t => t.MakerPN).HasColumnName("MakerPN");
            this.Property(t => t.RequestPurpose).HasColumnName("RequestPurpose");
            this.Property(t => t.LinksAndReferences).HasColumnName("LinksAndReferences");
            this.Property(t => t.ROHSCompliant).HasColumnName("ROHSCompliant");
            this.Property(t => t.OtherTechnicalDetails).HasColumnName("OtherTechnicalDetails");
            this.Property(t => t.NoteToBuyer).HasColumnName("NoteToBuyer");
            this.Property(t => t.Attachment1).HasColumnName("Attachment1");
            this.Property(t => t.Attachment2).HasColumnName("Attachment2");
            this.Property(t => t.Attachment3).HasColumnName("Attachment3");
            this.Property(t => t.TestEquipmentApplication).HasColumnName("TestEquipmentApplication");
            this.Property(t => t.RentalPeriodFrom).HasColumnName("RentalPeriodFrom");
            this.Property(t => t.RentalPeriodTo).HasColumnName("RentalPeriodTo");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.RFQId).HasColumnName("RFQId");
            this.Property(t => t.LineTypeId).HasColumnName("LineTypeId");
            this.Property(t => t.FormTypeId).HasColumnName("FormTypeId");
            this.Property(t => t.TestEquipmentPurchaseTypeId).HasColumnName("TestEquipmentPurchaseTypeId");
            this.Property(t => t.TestEquipmentCalibrationTypeId).HasColumnName("TestEquipmentCalibrationTypeId");
            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
            this.Property(t => t.QuantityUnitId).HasColumnName("QuantityUnitId");
            this.Property(t => t.DateCreatedOnUtc).HasColumnName("DateCreatedOnUtc");
            this.Property(t => t.DateUpdatedOnUtc).HasColumnName("DateUpdatedOnUtc");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");

            // Relationships
            this.HasRequired(t => t.currency)
                .WithMany(t => t.rfqlines)
                .HasForeignKey(d => d.CurrencyId);
            this.HasOptional(t => t.equipmentcalibrationtype)
                .WithMany(t => t.rfqlines)
                .HasForeignKey(d => d.TestEquipmentCalibrationTypeId);
            this.HasOptional(t => t.equipmentpurchasetype)
                .WithMany(t => t.rfqlines)
                .HasForeignKey(d => d.TestEquipmentPurchaseTypeId);
            this.HasRequired(t => t.rfq)
                .WithMany(t => t.rfqlines)
                .HasForeignKey(d => d.RFQId);
            this.HasRequired(t => t.user)
                .WithMany(t => t.rfqlines)
                .HasForeignKey(d => d.CreatedByUserId);
            this.HasRequired(t => t.rfqlinetype)
                .WithMany(t => t.rfqlines)
                .HasForeignKey(d => d.LineTypeId);
            this.HasRequired(t => t.user1)
                .WithMany(t => t.rfqlines1)
                .HasForeignKey(d => d.UpdatedByUserId);
            this.HasRequired(t => t.rfqlineformtype)
                .WithMany(t => t.rfqlines)
                .HasForeignKey(d => d.FormTypeId);
            this.HasRequired(t => t.uom)
                .WithMany(t => t.rfqlines)
                .HasForeignKey(d => d.QuantityUnitId);

        }
    }
}
