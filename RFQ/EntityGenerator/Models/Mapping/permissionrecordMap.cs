using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class permissionrecordMap : EntityTypeConfiguration<permissionrecord>
    {
        public permissionrecordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(1073741823);

            this.Property(t => t.SystemName)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Category)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("permissionrecord", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SystemName).HasColumnName("SystemName");
            this.Property(t => t.Category).HasColumnName("Category");

            // Relationships
            this.HasMany(t => t.userroles)
                .WithMany(t => t.permissionrecords)
                .Map(m =>
                    {
                        m.ToTable("permissionrecordrolemapping", "psp");
                        m.MapLeftKey("PermissionRecord_Id");
                        m.MapRightKey("UserRole_Id");
                    });


        }
    }
}
