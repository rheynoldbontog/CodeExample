using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class activitylogtypeMap : EntityTypeConfiguration<activitylogtype>
    {
        public activitylogtypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.SystemKeyword)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("activitylogtype", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SystemKeyword).HasColumnName("SystemKeyword");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Enabled).HasColumnName("Enabled");
        }
    }
}
