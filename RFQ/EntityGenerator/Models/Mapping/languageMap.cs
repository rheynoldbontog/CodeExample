using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class languageMap : EntityTypeConfiguration<language>
    {
        public languageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.LanguageCulture)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UniqueSeoCode)
                .HasMaxLength(2);

            this.Property(t => t.FlagImageFileName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("language", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.LanguageCulture).HasColumnName("LanguageCulture");
            this.Property(t => t.UniqueSeoCode).HasColumnName("UniqueSeoCode");
            this.Property(t => t.FlagImageFileName).HasColumnName("FlagImageFileName");
            this.Property(t => t.Rtl).HasColumnName("Rtl");
            this.Property(t => t.Published).HasColumnName("Published");
            this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");
        }
    }
}
