using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class supportcodeMap : EntityTypeConfiguration<supportcode>
    {
        public supportcodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.SupportCodes)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.SupportCodeDescription)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("supportcode", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SupportCodes).HasColumnName("SupportCodes");
            this.Property(t => t.SupportCodeDescription).HasColumnName("SupportCodeDescription");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.DateCreatedOnUtc).HasColumnName("DateCreatedOnUtc");
            this.Property(t => t.DateUpdatedOnUtc).HasColumnName("DateUpdatedOnUtc");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
        }
    }
}
