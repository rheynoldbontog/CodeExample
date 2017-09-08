using System.Data.Entity.ModelConfiguration;
using SSG.Core.Domain.Topics;

namespace SSG.Data.Mapping.Topics
{
    public class TopicMap : EntityTypeConfiguration<Topic>
    {
        public TopicMap()
        {
            this.ToTable("Topic");
            this.HasKey(t => t.Id);
            this.Property(t => t.SystemName).HasMaxLength(100);
            this.Property(t => t.Password).HasMaxLength(50);
            this.Property(t => t.Title).IsMaxLength();
            this.Property(t => t.Body).IsMaxLength();
            this.Property(t => t.MetaKeywords).IsMaxLength();
            this.Property(t => t.MetaDescription).IsMaxLength();
            this.Property(t => t.MetaTitle).IsMaxLength();
        }
    }
}
