﻿using System.Data.Entity.ModelConfiguration;
using SSG.Core.Domain.Logging;

namespace SSG.Data.Mapping.Logging
{
    public partial class ActivityLogMap : EntityTypeConfiguration<ActivityLog>
    {
        public ActivityLogMap()
        {
            this.ToTable("ActivityLog");
            this.HasKey(al => al.Id);
            this.Property(al => al.Comment).IsRequired().IsMaxLength();

            this.HasRequired(al => al.ActivityLogType)
                .WithMany(alt => alt.ActivityLog)
                .HasForeignKey(al => al.ActivityLogTypeId);

            this.HasRequired(al => al.User)
                .WithMany()
                .HasForeignKey(al => al.UserId);
        }
    }
}
