using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Modules;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TenantModuleConfiguration : IEntityTypeConfiguration<TenantModule>
{
    public void Configure(EntityTypeBuilder<TenantModule> builder)
    {
        builder.ToTable("tenant_modules");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ModuleKey).HasMaxLength(64).IsRequired();
        builder.Property(x => x.ModuleName).HasMaxLength(256).IsRequired();
    }
}

public static class TenantModuleConfigurationExtensions
{
    public static void ConfigureTenantModule(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TenantModuleConfiguration());
    }
}
