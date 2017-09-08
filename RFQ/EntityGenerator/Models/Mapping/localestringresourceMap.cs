using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class localestringresourceMap : EntityTypeConfiguration<localestringresource>
    {
        public localestringresourceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ResourceName)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.ResourceValue)
                .IsRequired()
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("localestringresource", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LanguageId).HasColumnName("LanguageId");
            this.Property(t => t.ResourceName).HasColumnName("ResourceName");
            this.Property(t => t.ResourceValue).HasColumnName("ResourceValue");

            // Relationships
            this.HasRequired(t => t.language)
                .WithMany(t => t.localestringresources)
                .HasForeignKey(d => d.LanguageId);

        }
    }
}
