using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class measureweightMap : EntityTypeConfiguration<measureweight>
    {
        public measureweightMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.SystemKeyword)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("measureweight", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SystemKeyword).HasColumnName("SystemKeyword");
            this.Property(t => t.Ratio).HasColumnName("Ratio");
            this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");
        }
    }
}
