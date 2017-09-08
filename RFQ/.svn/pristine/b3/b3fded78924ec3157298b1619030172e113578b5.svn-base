using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class localizedpropertyMap : EntityTypeConfiguration<localizedproperty>
    {
        public localizedpropertyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.LocaleKeyGroup)
                .IsRequired()
                .HasMaxLength(400);

            this.Property(t => t.LocaleKey)
                .IsRequired()
                .HasMaxLength(400);

            this.Property(t => t.LocaleValue)
                .IsRequired()
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("localizedproperty", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.LanguageId).HasColumnName("LanguageId");
            this.Property(t => t.LocaleKeyGroup).HasColumnName("LocaleKeyGroup");
            this.Property(t => t.LocaleKey).HasColumnName("LocaleKey");
            this.Property(t => t.LocaleValue).HasColumnName("LocaleValue");

            // Relationships
            this.HasRequired(t => t.language)
                .WithMany(t => t.localizedproperties)
                .HasForeignKey(d => d.LanguageId);

        }
    }
}
