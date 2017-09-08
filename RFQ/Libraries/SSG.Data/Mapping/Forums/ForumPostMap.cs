using System.Data.Entity.ModelConfiguration;
using SSG.Core.Domain.Forums;

namespace SSG.Data.Mapping.Forums
{
    public partial class ForumPostMap : EntityTypeConfiguration<ForumPost>
    {
        public ForumPostMap()
        {
            this.ToTable("ForumsPost");
            this.HasKey(fp => fp.Id);
            this.Property(fp => fp.Text).IsRequired().IsMaxLength();
            this.Property(fp => fp.IPAddress).HasMaxLength(100);

            this.HasRequired(fp => fp.ForumTopic)
                .WithMany()
                .HasForeignKey(fp => fp.TopicId);

            this.HasRequired(fp => fp.User)
               .WithMany(c => c.ForumPosts)
               .HasForeignKey(fp => fp.UserId)
               .WillCascadeOnDelete(false);
        }
    }
}
