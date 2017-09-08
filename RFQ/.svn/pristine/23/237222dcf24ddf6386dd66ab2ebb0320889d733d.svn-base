using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class activitylogMap : EntityTypeConfiguration<activitylog>
    {
        public activitylogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Comment)
                .IsRequired()
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("activitylog", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ActivityLogTypeId).HasColumnName("ActivityLogTypeId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Comment).HasColumnName("Comment");
            this.Property(t => t.CreatedOnUtc).HasColumnName("CreatedOnUtc");

            // Relationships
            this.HasRequired(t => t.activitylogtype)
                .WithMany(t => t.activitylogs)
                .HasForeignKey(d => d.ActivityLogTypeId);
            this.HasRequired(t => t.user)
                .WithMany(t => t.activitylogs)
                .HasForeignKey(d => d.UserId);

        }
    }
}
