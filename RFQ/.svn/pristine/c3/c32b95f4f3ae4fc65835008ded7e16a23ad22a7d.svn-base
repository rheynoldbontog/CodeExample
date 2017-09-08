using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class forumspostMap : EntityTypeConfiguration<forumspost>
    {
        public forumspostMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Text)
                .IsRequired()
                .HasMaxLength(1073741823);

            this.Property(t => t.IPAddress)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("forumspost", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TopicId).HasColumnName("TopicId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Text).HasColumnName("Text");
            this.Property(t => t.IPAddress).HasColumnName("IPAddress");
            this.Property(t => t.CreatedOnUtc).HasColumnName("CreatedOnUtc");
            this.Property(t => t.UpdatedOnUtc).HasColumnName("UpdatedOnUtc");

            // Relationships
            this.HasRequired(t => t.forumstopic)
                .WithMany(t => t.forumsposts)
                .HasForeignKey(d => d.TopicId);
            this.HasRequired(t => t.user)
                .WithMany(t => t.forumsposts)
                .HasForeignKey(d => d.UserId);

        }
    }
}
