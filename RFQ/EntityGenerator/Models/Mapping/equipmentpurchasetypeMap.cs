using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class equipmentpurchasetypeMap : EntityTypeConfiguration<equipmentpurchasetype>
    {
        public equipmentpurchasetypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("equipmentpurchasetype", "rfq");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.DateCreatedOnUtc).HasColumnName("DateCreatedOnUtc");
            this.Property(t => t.DateUpdatedOnUtc).HasColumnName("DateUpdatedOnUtc");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");

            // Relationships
            this.HasRequired(t => t.user)
                .WithMany(t => t.equipmentpurchasetypes)
                .HasForeignKey(d => d.CreatedByUserId);
            this.HasRequired(t => t.user1)
                .WithMany(t => t.equipmentpurchasetypes1)
                .HasForeignKey(d => d.UpdatedByUserId);

        }
    }
}
