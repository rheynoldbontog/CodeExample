using System.Data.Entity.ModelConfiguration;
using SSG.Plugin.Feed.Froogle.Domain;

namespace SSG.Plugin.Feed.Froogle.Data
{
    public partial class GoogleProductRecordMap : EntityTypeConfiguration<GoogleProductRecord>
    {
        public GoogleProductRecordMap()
        {
            this.ToTable("GoogleProduct");
            this.HasKey(x => x.Id);
        }
    }
}