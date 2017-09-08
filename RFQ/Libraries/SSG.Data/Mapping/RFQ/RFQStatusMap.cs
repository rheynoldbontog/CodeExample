using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;
using System.Data.Entity.ModelConfiguration;

namespace SSG.Data.Mapping.RFQ
{
    public class RFQStatusMap : EntityTypeConfiguration<RFQStatus>
    {
        public RFQStatusMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Property
            this.Property(t => t.Name).IsRequired().HasMaxLength(45).HasColumnName("Name");

            // References
            this.HasMany(t => t.QuoteRequests).WithRequired(t => t.RFQStatus).HasForeignKey(t => t.RFQStatusId).WillCascadeOnDelete(false);

            // Table & Column Mappings
            this.ToTable("rfqstatus", "rfq");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.DateCreatedOnUtc).HasColumnName("DateCreatedOnUtc");
            this.Property(t => t.DateUpdatedOnUtc).HasColumnName("DateUpdatedOnUtc");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
        }
    }
}
