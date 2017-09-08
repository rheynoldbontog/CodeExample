using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class quotationMap : EntityTypeConfiguration<quotation>
    {
        public quotationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.QuotationNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.QuoteItemDescription)
                .HasMaxLength(100);

            this.Property(t => t.MakerPN)
                .HasMaxLength(50);

            this.Property(t => t.PaymentTerms)
                .HasMaxLength(100);

            this.Property(t => t.Supplier)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.DeliveryLeadTime)
                .HasMaxLength(45);

            this.Property(t => t.WarrantyPeriod)
                .HasMaxLength(45);

            this.Property(t => t.BuyerRemarks)
                .HasMaxLength(1000);

            this.Property(t => t.RequesterRemarks)
                .HasMaxLength(1000);

            this.Property(t => t.QuotationAttachment)
                .HasMaxLength(100);

            this.Property(t => t.ROHSDocument)
                .HasMaxLength(100);

            this.Property(t => t.MSDSAttachment)
                .HasMaxLength(100);

            this.Property(t => t.OtherAttachment1)
                .HasMaxLength(100);

            this.Property(t => t.OtherAttachment2)
                .HasMaxLength(100);

            this.Property(t => t.OtherAttachment3)
                .HasMaxLength(100);

            this.Property(t => t.OtherAttachment4)
                .HasMaxLength(100);

            this.Property(t => t.OtherAttachment5)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("quotation", "rfq");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.QuotationNo).HasColumnName("QuotationNo");
            this.Property(t => t.QuoteUploadDate).HasColumnName("QuoteUploadDate");
            this.Property(t => t.QuoteItemDescription).HasColumnName("QuoteItemDescription");
            this.Property(t => t.MakerPN).HasColumnName("MakerPN");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.DiscountedPrice).HasColumnName("DiscountedPrice");
            this.Property(t => t.MinimumOrderQuantity).HasColumnName("MinimumOrderQuantity");
            this.Property(t => t.PaymentTerms).HasColumnName("PaymentTerms");
            this.Property(t => t.Supplier).HasColumnName("Supplier");
            this.Property(t => t.DeliveryLeadTime).HasColumnName("DeliveryLeadTime");
            this.Property(t => t.WarrantyPeriod).HasColumnName("WarrantyPeriod");
            this.Property(t => t.BuyerRemarks).HasColumnName("BuyerRemarks");
            this.Property(t => t.RequesterRemarks).HasColumnName("RequesterRemarks");
            this.Property(t => t.QuotationAttachment).HasColumnName("QuotationAttachment");
            this.Property(t => t.ROHSDocument).HasColumnName("ROHSDocument");
            this.Property(t => t.MSDSAttachment).HasColumnName("MSDSAttachment");
            this.Property(t => t.OtherAttachment1).HasColumnName("OtherAttachment1");
            this.Property(t => t.OtherAttachment2).HasColumnName("OtherAttachment2");
            this.Property(t => t.OtherAttachment3).HasColumnName("OtherAttachment3");
            this.Property(t => t.OtherAttachment4).HasColumnName("OtherAttachment4");
            this.Property(t => t.OtherAttachment5).HasColumnName("OtherAttachment5");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.RFQLineId).HasColumnName("RFQLineId");
            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
            this.Property(t => t.DateCreatedOnUtc).HasColumnName("DateCreatedOnUtc");
            this.Property(t => t.DateUpdatedOnUtc).HasColumnName("DateUpdatedOnUtc");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");

            // Relationships
            this.HasRequired(t => t.currency)
                .WithMany(t => t.quotations)
                .HasForeignKey(d => d.CurrencyId);
            this.HasRequired(t => t.user)
                .WithMany(t => t.quotations)
                .HasForeignKey(d => d.CreatedByUserId);
            this.HasRequired(t => t.rfqline)
                .WithMany(t => t.quotations)
                .HasForeignKey(d => d.RFQLineId);
            this.HasRequired(t => t.user1)
                .WithMany(t => t.quotations1)
                .HasForeignKey(d => d.UpdatedByUserId);

        }
    }
}
