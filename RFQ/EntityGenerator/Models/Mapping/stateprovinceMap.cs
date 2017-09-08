using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class stateprovinceMap : EntityTypeConfiguration<stateprovince>
    {
        public stateprovinceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Abbreviation)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("stateprovince", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Abbreviation).HasColumnName("Abbreviation");
            this.Property(t => t.Published).HasColumnName("Published");
            this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");

            // Relationships
            this.HasRequired(t => t.country)
                .WithMany(t => t.stateprovinces)
                .HasForeignKey(d => d.CountryId);

        }
    }
}
