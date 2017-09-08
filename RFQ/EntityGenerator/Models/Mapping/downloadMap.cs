using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class downloadMap : EntityTypeConfiguration<download>
    {
        public downloadMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DownloadUrl)
                .HasMaxLength(1073741823);

            this.Property(t => t.ContentType)
                .HasMaxLength(1073741823);

            this.Property(t => t.Filename)
                .HasMaxLength(1073741823);

            this.Property(t => t.Extension)
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("download", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DownloadGuid).HasColumnName("DownloadGuid");
            this.Property(t => t.UseDownloadUrl).HasColumnName("UseDownloadUrl");
            this.Property(t => t.DownloadUrl).HasColumnName("DownloadUrl");
            this.Property(t => t.DownloadBinary).HasColumnName("DownloadBinary");
            this.Property(t => t.ContentType).HasColumnName("ContentType");
            this.Property(t => t.Filename).HasColumnName("Filename");
            this.Property(t => t.Extension).HasColumnName("Extension");
            this.Property(t => t.IsNew).HasColumnName("IsNew");
        }
    }
}
