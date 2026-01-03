using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TenantSettingConfiguration : IEntityTypeConfiguration<TenantSetting>
{
    public void Configure(EntityTypeBuilder<TenantSetting> builder)
    {
        builder.ToTable("tenant_settings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.SettingKey).HasMaxLength(255).IsRequired();
        builder.HasIndex(x => x.SettingKey).IsUnique();
        builder.Property(x => x.SettingValue);
        builder.Property(x => x.SettingType).HasMaxLength(20).HasDefaultValue("string");
        builder.Property(x => x.Category).HasMaxLength(100);
        builder.Property(x => x.Description);
        builder.Property(x => x.IsUserEditable).HasDefaultValue(true);
        builder.HasIndex(x => x.Category);
    }
}

public static class TenantSettingConfigurationExtensions
{
    public static void ConfigureTenantSetting(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TenantSettingConfiguration());
    }
}
