using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class pollMap : EntityTypeConfiguration<poll>
    {
        public pollMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(1073741823);

            this.Property(t => t.SystemKeyword)
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("poll", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LanguageId).HasColumnName("LanguageId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SystemKeyword).HasColumnName("SystemKeyword");
            this.Property(t => t.Published).HasColumnName("Published");
            this.Property(t => t.ShowOnHomePage).HasColumnName("ShowOnHomePage");
            this.Property(t => t.AllowGuestsToVote).HasColumnName("AllowGuestsToVote");
            this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");
            this.Property(t => t.StartDateUtc).HasColumnName("StartDateUtc");
            this.Property(t => t.EndDateUtc).HasColumnName("EndDateUtc");

            // Relationships
            this.HasRequired(t => t.language)
                .WithMany(t => t.polls)
                .HasForeignKey(d => d.LanguageId);

        }
    }
}
