using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class pollanswerMap : EntityTypeConfiguration<pollanswer>
    {
        public pollanswerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("pollanswer", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PollId).HasColumnName("PollId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.NumberOfVotes).HasColumnName("NumberOfVotes");
            this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");

            // Relationships
            this.HasRequired(t => t.poll)
                .WithMany(t => t.pollanswers)
                .HasForeignKey(d => d.PollId);

        }
    }
}
