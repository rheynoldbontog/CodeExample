using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using SSG.Core.Domain.PSP;

namespace SSG.Data.Mapping.PSP
{
    class SupportCodeMap : EntityTypeConfiguration<SupportCode>
    {

        public SupportCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.SupportCodes)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.SupportCodeDescription)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("supportcode", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SupportCodes).HasColumnName("SupportCodes");
            this.Property(t => t.SupportCodeDescription).HasColumnName("SupportCodeDescription");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.DateCreatedOnUtc).HasColumnName("DateCreatedOnUtc");
            this.Property(t => t.DateUpdatedOnUtc).HasColumnName("DateUpdatedOnUtc");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
        }
    }
}
