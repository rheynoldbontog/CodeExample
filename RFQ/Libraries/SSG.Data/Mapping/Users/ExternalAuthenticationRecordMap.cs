using System.Data.Entity.ModelConfiguration;
using SSG.Core.Domain.Users;

namespace SSG.Data.Mapping.Users
{
    public partial class ExternalAuthenticationRecordMap : EntityTypeConfiguration<ExternalAuthenticationRecord>
    {
        public ExternalAuthenticationRecordMap()
        {
            this.ToTable("ExternalAuthenticationRecord");

            this.HasKey(ear => ear.Id);
            this.Property(ear => ear.Email).HasMaxLength(100);
            this.Property(ear => ear.ExternalIdentifier).HasMaxLength(255);
            this.Property(ear => ear.ExternalDisplayIdentifier).HasMaxLength(255);
            this.Property(ear => ear.OAuthToken).HasMaxLength(255);
            this.Property(ear => ear.OAuthAccessToken).HasMaxLength(255);
            this.Property(ear => ear.ProviderSystemName).HasMaxLength(100);

            this.HasRequired(ear => ear.User)
                .WithMany(c => c.ExternalAuthenticationRecords)
                .HasForeignKey(ear => ear.UserId);

        }
    }
}