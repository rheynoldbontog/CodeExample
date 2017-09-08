using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class usercontentMap : EntityTypeConfiguration<usercontent>
    {
        public usercontentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.IpAddress)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("usercontent", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IpAddress).HasColumnName("IpAddress");
            this.Property(t => t.IsApproved).HasColumnName("IsApproved");
            this.Property(t => t.CreatedOnUtc).HasColumnName("CreatedOnUtc");
            this.Property(t => t.UpdatedOnUtc).HasColumnName("UpdatedOnUtc");

            // Relationships
            this.HasRequired(t => t.user)
                .WithMany(t => t.usercontents)
                .HasForeignKey(d => d.UserId);

        }
    }
}
