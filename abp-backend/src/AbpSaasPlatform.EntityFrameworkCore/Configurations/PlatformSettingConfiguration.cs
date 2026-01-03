using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Settings;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class PlatformSettingConfiguration : IEntityTypeConfiguration<PlatformSetting>
{
    public void Configure(EntityTypeBuilder<PlatformSetting> builder)
    {
        builder.ToTable("platform_settings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.SettingKey).HasMaxLength(255).IsRequired();
        builder.HasIndex(x => x.SettingKey).IsUnique();
        builder.Property(x => x.SettingValue);
        builder.Property(x => x.SettingType).HasMaxLength(20).HasDefaultValue("string");
        builder.Property(x => x.Category).HasMaxLength(100);
        builder.Property(x => x.Description);
        builder.Property(x => x.IsSensitive).HasDefaultValue(false);
        builder.HasIndex(x => x.Category);
    }
}

public static class PlatformSettingConfigurationExtensions
{
    public static void ConfigurePlatformSetting(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new PlatformSettingConfiguration());
    }
}
