using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Modules;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class ModuleConfigurationConfiguration : IEntityTypeConfiguration<ModuleConfiguration>
{
    public void Configure(EntityTypeBuilder<ModuleConfiguration> builder)
    {
        builder.ToTable("tenant_module_settings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ModuleCode).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.ModuleCode).IsUnique();
        builder.Property(x => x.Settings);
        builder.Property(x => x.CustomFields);
        builder.Property(x => x.WorkflowConfig);
        builder.Property(x => x.EnabledFeatures);
        builder.Property(x => x.DisabledFeatures);
        builder.Property(x => x.DashboardConfig);
        builder.Property(x => x.ListViewConfig);
        builder.Property(x => x.FormLayoutConfig);
    }
}

public static class ModuleConfigurationConfigurationExtensions
{
    public static void ConfigureModuleConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ModuleConfigurationConfiguration());
    }
}
