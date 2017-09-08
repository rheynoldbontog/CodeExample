using System.Data.Entity.ModelConfiguration;
using SSG.Core.Domain.Forums;

namespace SSG.Data.Mapping.Forums
{
    public partial class ForumGroupMap : EntityTypeConfiguration<ForumGroup>
    {
        public ForumGroupMap()
        {
            this.ToTable("ForumsGroup");
            this.HasKey(fg => fg.Id);
            this.Property(fg => fg.Name).IsRequired().HasMaxLength(200);
            this.Property(fg => fg.Description).IsMaxLength();
        }
    }
}
