using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class forumsforumMap : EntityTypeConfiguration<forumsforum>
    {
        public forumsforumMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Description)
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("forumsforum", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ForumGroupId).HasColumnName("ForumGroupId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.NumTopics).HasColumnName("NumTopics");
            this.Property(t => t.NumPosts).HasColumnName("NumPosts");
            this.Property(t => t.LastTopicId).HasColumnName("LastTopicId");
            this.Property(t => t.LastPostId).HasColumnName("LastPostId");
            this.Property(t => t.LastPostUserId).HasColumnName("LastPostUserId");
            this.Property(t => t.LastPostTime).HasColumnName("LastPostTime");
            this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");
            this.Property(t => t.CreatedOnUtc).HasColumnName("CreatedOnUtc");
            this.Property(t => t.UpdatedOnUtc).HasColumnName("UpdatedOnUtc");

            // Relationships
            this.HasRequired(t => t.forumsgroup)
                .WithMany(t => t.forumsforums)
                .HasForeignKey(d => d.ForumGroupId);

        }
    }
}
