using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class newslettersubscriptionMap : EntityTypeConfiguration<newslettersubscription>
    {
        public newslettersubscriptionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("newslettersubscription", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.NewsLetterSubscriptionGuid).HasColumnName("NewsLetterSubscriptionGuid");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.CreatedOnUtc).HasColumnName("CreatedOnUtc");
        }
    }
}
