using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using SSG.Core.Domain.RFQ;

namespace SSG.Data.Mapping.RFQ
{
    public class FileAttachmentMap : EntityTypeConfiguration<FileAttachment>
    {
        public FileAttachmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Filename).IsOptional().HasMaxLength(150).HasColumnName("Filename");
            this.Property(t => t.FileUrl).IsOptional().HasMaxLength(150).HasColumnName("FileUrl");
            this.Property(t => t.FileAttachmentType).IsRequired().HasColumnName("FileAttachmentType");
            
            // Table & Column Mappings
            this.ToTable("fileattachment", "rfq");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.DateCreatedOnUtc).HasColumnName("DateCreatedOnUtc");
            this.Property(t => t.DateUpdatedOnUtc).HasColumnName("DateUpdatedOnUtc");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
        }
    }
}
