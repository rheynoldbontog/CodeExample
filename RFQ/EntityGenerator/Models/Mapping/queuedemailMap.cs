using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class queuedemailMap : EntityTypeConfiguration<queuedemail>
    {
        public queuedemailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.From)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.FromName)
                .HasMaxLength(500);

            this.Property(t => t.To)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.ToName)
                .HasMaxLength(500);

            this.Property(t => t.CC)
                .HasMaxLength(500);

            this.Property(t => t.Bcc)
                .HasMaxLength(500);

            this.Property(t => t.Subject)
                .HasMaxLength(1000);

            this.Property(t => t.Body)
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("queuedemail", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Priority).HasColumnName("Priority");
            this.Property(t => t.From).HasColumnName("From");
            this.Property(t => t.FromName).HasColumnName("FromName");
            this.Property(t => t.To).HasColumnName("To");
            this.Property(t => t.ToName).HasColumnName("ToName");
            this.Property(t => t.CC).HasColumnName("CC");
            this.Property(t => t.Bcc).HasColumnName("Bcc");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.Body).HasColumnName("Body");
            this.Property(t => t.CreatedOnUtc).HasColumnName("CreatedOnUtc");
            this.Property(t => t.SentTries).HasColumnName("SentTries");
            this.Property(t => t.SentOnUtc).HasColumnName("SentOnUtc");
            this.Property(t => t.EmailAccountId).HasColumnName("EmailAccountId");

            // Relationships
            this.HasRequired(t => t.emailaccount)
                .WithMany(t => t.queuedemails)
                .HasForeignKey(d => d.EmailAccountId);

        }
    }
}
