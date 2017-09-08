using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using SSG.Core.Domain.RFQ;

namespace SSG.Data.Mapping.RFQ
{
    public class RFQLineFormTypeMap : EntityTypeConfiguration<RFQLineFormType>
    {
        public RFQLineFormTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Property
            this.Property(t => t.Name).IsRequired().HasMaxLength(45).HasColumnName("Name");

            //Relationships
            this.HasMany(t => t.Lines).WithRequired(l => l.FormType).HasForeignKey(l => l.FormTypeId).WillCascadeOnDelete(false);

            // Table & Column Mappings
            this.ToTable("rfqlineformtype", "rfq");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.DateCreatedOnUtc).HasColumnName("DateCreatedOnUtc");
            this.Property(t => t.DateUpdatedOnUtc).HasColumnName("DateUpdatedOnUtc");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
        }
    }
}
