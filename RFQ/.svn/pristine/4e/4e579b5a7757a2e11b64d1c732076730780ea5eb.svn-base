using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class pictureMap : EntityTypeConfiguration<picture>
    {
        public pictureMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.MimeType)
                .IsRequired()
                .HasMaxLength(40);

            this.Property(t => t.SeoFilename)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("picture", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PictureBinary).HasColumnName("PictureBinary");
            this.Property(t => t.MimeType).HasColumnName("MimeType");
            this.Property(t => t.SeoFilename).HasColumnName("SeoFilename");
            this.Property(t => t.IsNew).HasColumnName("IsNew");
        }
    }
}
