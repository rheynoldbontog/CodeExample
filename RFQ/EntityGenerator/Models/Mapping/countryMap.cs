using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class countryMap : EntityTypeConfiguration<country>
    {
        public countryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.TwoLetterIsoCode)
                .HasMaxLength(2);

            this.Property(t => t.ThreeLetterIsoCode)
                .HasMaxLength(3);

            // Table & Column Mappings
            this.ToTable("country", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.AllowsBilling).HasColumnName("AllowsBilling");
            this.Property(t => t.AllowsShipping).HasColumnName("AllowsShipping");
            this.Property(t => t.TwoLetterIsoCode).HasColumnName("TwoLetterIsoCode");
            this.Property(t => t.ThreeLetterIsoCode).HasColumnName("ThreeLetterIsoCode");
            this.Property(t => t.NumericIsoCode).HasColumnName("NumericIsoCode");
            this.Property(t => t.Published).HasColumnName("Published");
            this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");
        }
    }
}
