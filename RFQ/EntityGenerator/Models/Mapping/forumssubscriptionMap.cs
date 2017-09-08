using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class forumssubscriptionMap : EntityTypeConfiguration<forumssubscription>
    {
        public forumssubscriptionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("forumssubscription", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SubscriptionGuid).HasColumnName("SubscriptionGuid");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.ForumId).HasColumnName("ForumId");
            this.Property(t => t.TopicId).HasColumnName("TopicId");
            this.Property(t => t.CreatedOnUtc).HasColumnName("CreatedOnUtc");

            // Relationships
            this.HasRequired(t => t.user)
                .WithMany(t => t.forumssubscriptions)
                .HasForeignKey(d => d.UserId);

        }
    }
}
