using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TenantModuleSettingConfiguration : IEntityTypeConfiguration<TenantModuleSetting>
{
    public void Configure(EntityTypeBuilder<TenantModuleSetting> builder)
    {
        builder.ToTable("tenant_module_settings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ModuleCode).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.ModuleCode).IsUnique();
    }
}

public static class TenantModuleSettingConfigurationExtensions
{
    public static void ConfigureTenantModuleSetting(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TenantModuleSettingConfiguration());
    }
}
