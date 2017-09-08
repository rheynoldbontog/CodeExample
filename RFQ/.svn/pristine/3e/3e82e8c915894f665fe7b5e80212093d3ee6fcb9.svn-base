using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class userroleMap : EntityTypeConfiguration<userrole>
    {
        public userroleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.SystemName)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("userrole", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.FreeShipping).HasColumnName("FreeShipping");
            this.Property(t => t.TaxExempt).HasColumnName("TaxExempt");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.IsSystemRole).HasColumnName("IsSystemRole");
            this.Property(t => t.SystemName).HasColumnName("SystemName");
        }
    }
}
