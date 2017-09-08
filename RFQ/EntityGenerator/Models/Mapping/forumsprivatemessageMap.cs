using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class forumsprivatemessageMap : EntityTypeConfiguration<forumsprivatemessage>
    {
        public forumsprivatemessageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Subject)
                .IsRequired()
                .HasMaxLength(450);

            this.Property(t => t.Text)
                .IsRequired()
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("forumsprivatemessage", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FromUserId).HasColumnName("FromUserId");
            this.Property(t => t.ToUserId).HasColumnName("ToUserId");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.Text).HasColumnName("Text");
            this.Property(t => t.IsRead).HasColumnName("IsRead");
            this.Property(t => t.IsDeletedByAuthor).HasColumnName("IsDeletedByAuthor");
            this.Property(t => t.IsDeletedByRecipient).HasColumnName("IsDeletedByRecipient");
            this.Property(t => t.CreatedOnUtc).HasColumnName("CreatedOnUtc");

            // Relationships
            this.HasRequired(t => t.user)
                .WithMany(t => t.forumsprivatemessages)
                .HasForeignKey(d => d.FromUserId);
            this.HasRequired(t => t.user1)
                .WithMany(t => t.forumsprivatemessages1)
                .HasForeignKey(d => d.ToUserId);

        }
    }
}
