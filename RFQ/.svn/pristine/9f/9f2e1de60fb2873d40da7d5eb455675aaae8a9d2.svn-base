using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class newscommentMap : EntityTypeConfiguration<newscomment>
    {
        public newscommentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CommentTitle)
                .HasMaxLength(1073741823);

            this.Property(t => t.CommentText)
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("newscomment", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CommentTitle).HasColumnName("CommentTitle");
            this.Property(t => t.CommentText).HasColumnName("CommentText");
            this.Property(t => t.NewsItemId).HasColumnName("NewsItemId");

            // Relationships
            this.HasRequired(t => t.news)
                .WithMany(t => t.newscomments)
                .HasForeignKey(d => d.NewsItemId);
            this.HasRequired(t => t.usercontent)
                .WithOptional(t => t.newscomment);

        }
    }
}
