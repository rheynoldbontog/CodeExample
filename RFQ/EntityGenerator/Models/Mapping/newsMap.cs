using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class newsMap : EntityTypeConfiguration<news>
    {
        public newsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(1073741823);

            this.Property(t => t.Short)
                .IsRequired()
                .HasMaxLength(1073741823);

            this.Property(t => t.Full)
                .IsRequired()
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("news", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LanguageId).HasColumnName("LanguageId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Short).HasColumnName("Short");
            this.Property(t => t.Full).HasColumnName("Full");
            this.Property(t => t.Published).HasColumnName("Published");
            this.Property(t => t.StartDateUtc).HasColumnName("StartDateUtc");
            this.Property(t => t.EndDateUtc).HasColumnName("EndDateUtc");
            this.Property(t => t.AllowComments).HasColumnName("AllowComments");
            this.Property(t => t.ApprovedCommentCount).HasColumnName("ApprovedCommentCount");
            this.Property(t => t.NotApprovedCommentCount).HasColumnName("NotApprovedCommentCount");
            this.Property(t => t.CreatedOnUtc).HasColumnName("CreatedOnUtc");

            // Relationships
            this.HasRequired(t => t.language)
                .WithMany(t => t.news)
                .HasForeignKey(d => d.LanguageId);

        }
    }
}
