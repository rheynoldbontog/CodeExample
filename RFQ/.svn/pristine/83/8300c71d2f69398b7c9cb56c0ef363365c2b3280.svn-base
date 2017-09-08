using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace EntityGenerator.Models.Mapping
{
    public class userMap : EntityTypeConfiguration<user>
    {
        public userMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Username)
                .HasMaxLength(1000);

            this.Property(t => t.Email)
                .HasMaxLength(1000);

            this.Property(t => t.Password)
                .HasMaxLength(50);

            this.Property(t => t.PasswordSalt)
                .HasMaxLength(1073741823);

            this.Property(t => t.AdminComment)
                .HasMaxLength(1073741823);

            this.Property(t => t.EmployeeId)
                .HasMaxLength(10);

            this.Property(t => t.TimeZoneId)
                .HasMaxLength(1073741823);

            this.Property(t => t.SystemName)
                .HasMaxLength(1073741823);

            this.Property(t => t.LastIpAddress)
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("user", "psp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserGuid).HasColumnName("UserGuid");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.PasswordFormatId).HasColumnName("PasswordFormatId");
            this.Property(t => t.PasswordSalt).HasColumnName("PasswordSalt");
            this.Property(t => t.AdminComment).HasColumnName("AdminComment");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.LanguageId).HasColumnName("LanguageId");
            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
            this.Property(t => t.TimeZoneId).HasColumnName("TimeZoneId");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.Deleted).HasColumnName("Deleted");
            this.Property(t => t.IsSystemAccount).HasColumnName("IsSystemAccount");
            this.Property(t => t.SystemName).HasColumnName("SystemName");
            this.Property(t => t.LastIpAddress).HasColumnName("LastIpAddress");
            this.Property(t => t.CreatedOnUtc).HasColumnName("CreatedOnUtc");
            this.Property(t => t.LastLoginDateUtc).HasColumnName("LastLoginDateUtc");
            this.Property(t => t.LastActivityDateUtc).HasColumnName("LastActivityDateUtc");
            this.Property(t => t.BillingAddress_Id).HasColumnName("BillingAddress_Id");
            this.Property(t => t.ShippingAddress_Id).HasColumnName("ShippingAddress_Id");

            // Relationships
            this.HasMany(t => t.addresses)
                .WithMany(t => t.users2)
                .Map(m =>
                    {
                        m.ToTable("useraddresses", "psp");
                        m.MapLeftKey("User_Id");
                        m.MapRightKey("Address_Id");
                    });

            this.HasMany(t => t.userroles)
                .WithMany(t => t.users)
                .Map(m =>
                    {
                        m.ToTable("useruserrolemapping", "psp");
                        m.MapLeftKey("User_Id");
                        m.MapRightKey("UserRole_Id");
                    });

            this.HasOptional(t => t.address)
                .WithMany(t => t.users)
                .HasForeignKey(d => d.BillingAddress_Id);
            this.HasOptional(t => t.address1)
                .WithMany(t => t.users1)
                .HasForeignKey(d => d.ShippingAddress_Id);
            this.HasOptional(t => t.currency)
                .WithMany(t => t.users)
                .HasForeignKey(d => d.CurrencyId);
            this.HasOptional(t => t.language)
                .WithMany(t => t.users)
                .HasForeignKey(d => d.LanguageId);

        }
    }
}
