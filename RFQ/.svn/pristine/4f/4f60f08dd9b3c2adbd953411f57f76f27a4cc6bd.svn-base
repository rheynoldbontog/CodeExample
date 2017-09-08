using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class currencyMap : EntityTypeConfiguration<currency>
    {
        public currencyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CurrencyCode)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.DisplayLocale)
                .HasMaxLength(50);

            this.Property(t => t.CustomFormatting)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("currency", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CurrencyCode).HasColumnName("CurrencyCode");
            this.Property(t => t.Rate).HasColumnName("Rate");
            this.Property(t => t.DisplayLocale).HasColumnName("DisplayLocale");
            this.Property(t => t.CustomFormatting).HasColumnName("CustomFormatting");
            this.Property(t => t.Published).HasColumnName("Published");
            this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");
            this.Property(t => t.CreatedOnUtc).HasColumnName("CreatedOnUtc");
            this.Property(t => t.UpdatedOnUtc).HasColumnName("UpdatedOnUtc");
        }
    }
}
