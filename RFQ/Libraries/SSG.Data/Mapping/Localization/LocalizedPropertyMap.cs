using System.Data.Entity.ModelConfiguration;
using SSG.Core.Domain.Localization;

namespace SSG.Data.Mapping.Localization
{
    public partial class LocalizedPropertyMap : EntityTypeConfiguration<LocalizedProperty>
    {
        public LocalizedPropertyMap()
        {
            this.ToTable("LocalizedProperty");
            this.HasKey(lp => lp.Id);

            this.Property(lp => lp.LocaleKeyGroup).IsRequired().HasMaxLength(400);
            this.Property(lp => lp.LocaleKey).IsRequired().HasMaxLength(400);
            this.Property(lp => lp.LocaleValue).IsRequired().IsMaxLength();
            
            this.HasRequired(lp => lp.Language)
                .WithMany()
                .HasForeignKey(lp => lp.LanguageId);
        }
    }
}