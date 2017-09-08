using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class messagetemplateMap : EntityTypeConfiguration<messagetemplate>
    {
        public messagetemplateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.BccEmailAddresses)
                .HasMaxLength(200);

            this.Property(t => t.Subject)
                .HasMaxLength(1000);

            this.Property(t => t.Body)
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("messagetemplate", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.BccEmailAddresses).HasColumnName("BccEmailAddresses");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.Body).HasColumnName("Body");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.EmailAccountId).HasColumnName("EmailAccountId");
        }
    }
}
