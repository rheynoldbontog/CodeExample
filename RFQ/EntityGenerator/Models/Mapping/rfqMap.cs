using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class rfqMap : EntityTypeConfiguration<rfq>
    {
        public rfqMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.RFQNo)
                .HasMaxLength(1073741823);

            this.Property(t => t.Department)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Subject)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.NoteToBuyer)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("rfq", "rfq");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RFQNo).HasColumnName("RFQNo");
            this.Property(t => t.Department).HasColumnName("Department");
            this.Property(t => t.NeedByDate).HasColumnName("NeedByDate");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.NoteToBuyer).HasColumnName("NoteToBuyer");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.RFQStatusId).HasColumnName("RFQStatusId");
            this.Property(t => t.BuyerId).HasColumnName("BuyerId");
            this.Property(t => t.RequestorId).HasColumnName("RequestorId");
            this.Property(t => t.DateCreatedOnUtc).HasColumnName("DateCreatedOnUtc");
            this.Property(t => t.DateUpdatedOnUtc).HasColumnName("DateUpdatedOnUtc");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");

            // Relationships
            this.HasRequired(t => t.user)
                .WithMany(t => t.rfqs)
                .HasForeignKey(d => d.BuyerId);
            this.HasRequired(t => t.user1)
                .WithMany(t => t.rfqs1)
                .HasForeignKey(d => d.CreatedByUserId);
            this.HasRequired(t => t.user2)
                .WithMany(t => t.rfqs2)
                .HasForeignKey(d => d.RequestorId);
            this.HasRequired(t => t.user3)
                .WithMany(t => t.rfqs3)
                .HasForeignKey(d => d.UpdatedByUserId);
            this.HasRequired(t => t.rfqstatu)
                .WithMany(t => t.rfqs)
                .HasForeignKey(d => d.RFQStatusId);

        }
    }
}
