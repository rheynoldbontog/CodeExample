using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class rfqstatushistoryMap : EntityTypeConfiguration<rfqstatushistory>
    {
        public rfqstatushistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("rfqstatushistory", "rfq");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RfqLineId).HasColumnName("RfqLineId");
            this.Property(t => t.RfqStatusId).HasColumnName("RfqStatusId");
            this.Property(t => t.DateCreatedOnUtc).HasColumnName("DateCreatedOnUtc");
            this.Property(t => t.DateUpdatedOnUtc).HasColumnName("DateUpdatedOnUtc");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
        }
    }
}
