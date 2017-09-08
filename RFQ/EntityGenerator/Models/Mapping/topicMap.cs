using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class topicMap : EntityTypeConfiguration<topic>
    {
        public topicMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.SystemName)
                .HasMaxLength(100);

            this.Property(t => t.Password)
                .HasMaxLength(50);

            this.Property(t => t.Title)
                .HasMaxLength(1073741823);

            this.Property(t => t.Body)
                .HasMaxLength(1073741823);

            this.Property(t => t.MetaKeywords)
                .HasMaxLength(1073741823);

            this.Property(t => t.MetaDescription)
                .HasMaxLength(1073741823);

            this.Property(t => t.MetaTitle)
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("topic", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SystemName).HasColumnName("SystemName");
            this.Property(t => t.IncludeInSitemap).HasColumnName("IncludeInSitemap");
            this.Property(t => t.IsPasswordProtected).HasColumnName("IsPasswordProtected");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Body).HasColumnName("Body");
            this.Property(t => t.MetaKeywords).HasColumnName("MetaKeywords");
            this.Property(t => t.MetaDescription).HasColumnName("MetaDescription");
            this.Property(t => t.MetaTitle).HasColumnName("MetaTitle");
        }
    }
}
