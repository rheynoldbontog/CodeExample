using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class externalauthenticationrecordMap : EntityTypeConfiguration<externalauthenticationrecord>
    {
        public externalauthenticationrecordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Email)
                .HasMaxLength(100);

            this.Property(t => t.ExternalIdentifier)
                .HasMaxLength(255);

            this.Property(t => t.ExternalDisplayIdentifier)
                .HasMaxLength(255);

            this.Property(t => t.OAuthToken)
                .HasMaxLength(255);

            this.Property(t => t.OAuthAccessToken)
                .HasMaxLength(255);

            this.Property(t => t.ProviderSystemName)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("externalauthenticationrecord", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.ExternalIdentifier).HasColumnName("ExternalIdentifier");
            this.Property(t => t.ExternalDisplayIdentifier).HasColumnName("ExternalDisplayIdentifier");
            this.Property(t => t.OAuthToken).HasColumnName("OAuthToken");
            this.Property(t => t.OAuthAccessToken).HasColumnName("OAuthAccessToken");
            this.Property(t => t.ProviderSystemName).HasColumnName("ProviderSystemName");

            // Relationships
            this.HasRequired(t => t.user)
                .WithMany(t => t.externalauthenticationrecords)
                .HasForeignKey(d => d.UserId);

        }
    }
}
