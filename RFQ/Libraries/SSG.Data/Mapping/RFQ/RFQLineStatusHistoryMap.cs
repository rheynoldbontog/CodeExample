using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;

namespace SSG.Data.Mapping.RFQ
{
    public class RFQLineStatusHistoryMap : EntityTypeConfiguration<RFQLineStatusHistory>
    {
        public RFQLineStatusHistoryMap()
        {
            //Primary Key
            this.HasKey(t => t.Id);
            
            //Properties
            this.HasRequired(t => t.RFQLine)
                .WithMany(t => t.StatusHistory)
                .HasForeignKey(t => t.RFQLineId)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.OldStatus)
                .WithMany()
                .HasForeignKey(t => t.OldStatusId)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.NewStatus)
                .WithMany()
                .HasForeignKey(t => t.NewStatusId)
                .WillCascadeOnDelete(false);

            // Table & Column Mappings
            this.ToTable("rfqlinestatushistory", "rfq");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.DateCreatedOnUtc).HasColumnName("DateCreatedOnUtc");
            this.Property(t => t.DateUpdatedOnUtc).HasColumnName("DateUpdatedOnUtc");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
        }
      
    }
}
