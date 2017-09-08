using System.Data.Entity.ModelConfiguration;
using SSG.Core.Domain.Users;

namespace SSG.Data.Mapping.Users
{
    public partial class UserContentMap : EntityTypeConfiguration<UserContent>
    {
        public UserContentMap()
        {
            this.ToTable("UserContent");

            this.HasKey(cc => cc.Id);
            this.Property(cc => cc.IpAddress).HasMaxLength(200);

            this.HasRequired(cc => cc.User)
                .WithMany(c => c.UserContent)
                .HasForeignKey(cc => cc.UserId);

        }
    }
}