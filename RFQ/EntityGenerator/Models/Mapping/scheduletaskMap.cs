using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class scheduletaskMap : EntityTypeConfiguration<scheduletask>
    {
        public scheduletaskMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(1073741823);

            this.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("scheduletask", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Seconds).HasColumnName("Seconds");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Enabled).HasColumnName("Enabled");
            this.Property(t => t.StopOnError).HasColumnName("StopOnError");
            this.Property(t => t.LastStartUtc).HasColumnName("LastStartUtc");
            this.Property(t => t.LastEndUtc).HasColumnName("LastEndUtc");
            this.Property(t => t.LastSuccessUtc).HasColumnName("LastSuccessUtc");
        }
    }
}
