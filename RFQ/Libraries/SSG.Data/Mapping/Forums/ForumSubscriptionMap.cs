using System.Data.Entity.ModelConfiguration;
using SSG.Core.Domain.Forums;

namespace SSG.Data.Mapping.Forums
{
    public partial class ForumSubscriptionMap : EntityTypeConfiguration<ForumSubscription>
    {
        public ForumSubscriptionMap()
        {
            this.ToTable("ForumsSubscription");
            this.HasKey(fs => fs.Id);

            this.HasRequired(fs => fs.User)
                .WithMany()
                .HasForeignKey(fs => fs.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
