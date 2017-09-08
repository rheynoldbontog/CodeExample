using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class logMap : EntityTypeConfiguration<log>
    {
        public logMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ShortMessage)
                .IsRequired()
                .HasMaxLength(1073741823);

            this.Property(t => t.FullMessage)
                .HasMaxLength(1073741823);

            this.Property(t => t.IpAddress)
                .HasMaxLength(200);

            this.Property(t => t.PageUrl)
                .HasMaxLength(1073741823);

            this.Property(t => t.ReferrerUrl)
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("log", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LogLevelId).HasColumnName("LogLevelId");
            this.Property(t => t.ShortMessage).HasColumnName("ShortMessage");
            this.Property(t => t.FullMessage).HasColumnName("FullMessage");
            this.Property(t => t.IpAddress).HasColumnName("IpAddress");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.PageUrl).HasColumnName("PageUrl");
            this.Property(t => t.ReferrerUrl).HasColumnName("ReferrerUrl");
            this.Property(t => t.CreatedOnUtc).HasColumnName("CreatedOnUtc");

            // Relationships
            this.HasOptional(t => t.user)
                .WithMany(t => t.logs)
                .HasForeignKey(d => d.UserId);

        }
    }
}
