﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using SSG.Core.Domain.RFQ;

namespace SSG.Data.Mapping.RFQ
{
    public class UOMMap : EntityTypeConfiguration<UOM>
    {
        public UOMMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Property
            this.Property(t => t.Name).IsRequired().HasMaxLength(45).HasColumnName("Name");

            //Relationships
            this.HasMany(t => t.Lines).WithRequired(l => l.QuantityUnit).HasForeignKey(t => t.QuantityUnitId).WillCascadeOnDelete(false);

            // Table & Column Mappings
            this.ToTable("uom", "rfq");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.DateCreatedOnUtc).HasColumnName("DateCreatedOnUtc");
            this.Property(t => t.DateUpdatedOnUtc).HasColumnName("DateUpdatedOnUtc");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
        }
    }
}
