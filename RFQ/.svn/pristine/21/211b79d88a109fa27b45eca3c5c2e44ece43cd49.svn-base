using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class addressMap : EntityTypeConfiguration<address>
    {
        public addressMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.FirstName)
                .HasMaxLength(1073741823);

            this.Property(t => t.LastName)
                .HasMaxLength(1073741823);

            this.Property(t => t.Email)
                .HasMaxLength(1073741823);

            this.Property(t => t.Company)
                .HasMaxLength(1073741823);

            this.Property(t => t.City)
                .HasMaxLength(1073741823);

            this.Property(t => t.Address1)
                .HasMaxLength(1073741823);

            this.Property(t => t.Address2)
                .HasMaxLength(1073741823);

            this.Property(t => t.ZipPostalCode)
                .HasMaxLength(1073741823);

            this.Property(t => t.PhoneNumber)
                .HasMaxLength(1073741823);

            this.Property(t => t.FaxNumber)
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("address", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Company).HasColumnName("Company");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.StateProvinceId).HasColumnName("StateProvinceId");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Address1).HasColumnName("Address1");
            this.Property(t => t.Address2).HasColumnName("Address2");
            this.Property(t => t.ZipPostalCode).HasColumnName("ZipPostalCode");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.FaxNumber).HasColumnName("FaxNumber");
            this.Property(t => t.CreatedOnUtc).HasColumnName("CreatedOnUtc");

            // Relationships
            this.HasOptional(t => t.country)
                .WithMany(t => t.addresses)
                .HasForeignKey(d => d.CountryId);
            this.HasOptional(t => t.stateprovince)
                .WithMany(t => t.addresses)
                .HasForeignKey(d => d.StateProvinceId);

        }
    }
}
