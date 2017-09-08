using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class testMap : EntityTypeConfiguration<test>
    {
        public testMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Test1)
                .HasMaxLength(50);

            this.Property(t => t.Test2)
                .HasMaxLength(50);

            this.Property(t => t.Test3)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("test", "rfq");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Test1).HasColumnName("Test1");
            this.Property(t => t.Test2).HasColumnName("Test2");
            this.Property(t => t.Test3).HasColumnName("Test3");
        }
    }
}
