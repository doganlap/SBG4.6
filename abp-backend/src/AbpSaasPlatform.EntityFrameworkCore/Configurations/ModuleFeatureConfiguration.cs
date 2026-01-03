using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Modules;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class ModuleFeatureConfiguration : IEntityTypeConfiguration<ModuleFeature>
{
    public void Configure(EntityTypeBuilder<ModuleFeature> builder)
    {
        builder.ToTable("module_features");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ModuleId).IsRequired();
        builder.Property(x => x.FeatureCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.FeatureName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description);
        builder.Property(x => x.IsEnabled).HasDefaultValue(true);
        builder.Property(x => x.IsPremium).HasDefaultValue(false);
        builder.Property(x => x.Configuration);
        builder.HasOne(x => x.Module)
            .WithMany()
            .HasForeignKey(x => x.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public static class ModuleFeatureConfigurationExtensions
{
    public static void ConfigureModuleFeature(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ModuleFeatureConfiguration());
    }
}
