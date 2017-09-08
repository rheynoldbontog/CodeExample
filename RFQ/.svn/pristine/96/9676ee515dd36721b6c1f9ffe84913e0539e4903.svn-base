using System.Data.Entity.ModelConfiguration;
using SSG.Core.Domain.Common;
using SSG.Core.Domain.Users;

namespace SSG.Data.Mapping.Users
{
    public partial class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable("User");
            this.HasKey(c => c.Id);
            this.Property(u => u.Username).HasMaxLength(1000);
            this.Property(u => u.Email).HasMaxLength(1000);
            this.Property(u => u.Password).HasMaxLength(50);
            this.Property(c => c.AdminComment).IsMaxLength();
            this.Property(u => u.EmployeeId).HasMaxLength(10);

            this.Ignore(u => u.PasswordFormat);

            this.HasOptional(c => c.Language)
                .WithMany()
                .HasForeignKey(c => c.LanguageId).WillCascadeOnDelete(false);

            this.HasOptional(c => c.Currency)
                .WithMany()
                .HasForeignKey(c => c.CurrencyId).WillCascadeOnDelete(false);

            this.HasOptional(c => c.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(c => c.DepartmentId).WillCascadeOnDelete(false);

            this.HasMany(c => c.UserRoles)
                .WithMany()
                .Map(m => m.ToTable("UserUserRoleMapping"));

            this.HasMany(c => c.BuyerDepartments)
                .WithMany(d => d.Buyers)
                .Map(m => m.ToTable("buyerdepartmentmapping"));

            this.HasMany<Address>(c => c.Addresses)
                .WithMany()
                .Map(m => m.ToTable("UserAddresses"));
            this.HasOptional<Address>(c => c.BillingAddress);
            this.HasOptional<Address>(c => c.ShippingAddress);
        }
    }
}