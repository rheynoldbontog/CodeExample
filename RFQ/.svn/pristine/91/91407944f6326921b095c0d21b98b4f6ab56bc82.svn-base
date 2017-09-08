using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class forumstopicMap : EntityTypeConfiguration<forumstopic>
    {
        public forumstopicMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Subject)
                .IsRequired()
                .HasMaxLength(450);

            // Table & Column Mappings
            this.ToTable("forumstopic", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ForumId).HasColumnName("ForumId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.TopicTypeId).HasColumnName("TopicTypeId");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.NumPosts).HasColumnName("NumPosts");
            this.Property(t => t.Views).HasColumnName("Views");
            this.Property(t => t.LastPostId).HasColumnName("LastPostId");
            this.Property(t => t.LastPostUserId).HasColumnName("LastPostUserId");
            this.Property(t => t.LastPostTime).HasColumnName("LastPostTime");
            this.Property(t => t.CreatedOnUtc).HasColumnName("CreatedOnUtc");
            this.Property(t => t.UpdatedOnUtc).HasColumnName("UpdatedOnUtc");

            // Relationships
            this.HasRequired(t => t.forumsforum)
                .WithMany(t => t.forumstopics)
                .HasForeignKey(d => d.ForumId);
            this.HasRequired(t => t.user)
                .WithMany(t => t.forumstopics)
                .HasForeignKey(d => d.UserId);

        }
    }
}
