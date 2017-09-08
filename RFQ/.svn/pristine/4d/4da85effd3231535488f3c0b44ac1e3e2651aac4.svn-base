using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class genericattributeMap : EntityTypeConfiguration<genericattribute>
    {
        public genericattributeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.KeyGroup)
                .IsRequired()
                .HasMaxLength(400);

            this.Property(t => t.Key)
                .IsRequired()
                .HasMaxLength(400);

            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("genericattribute", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.KeyGroup).HasColumnName("KeyGroup");
            this.Property(t => t.Key).HasColumnName("Key");
            this.Property(t => t.Value).HasColumnName("Value");
        }
    }
}
