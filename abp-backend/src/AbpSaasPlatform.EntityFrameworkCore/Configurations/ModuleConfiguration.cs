using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Modules;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("modules");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ModuleCode).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.ModuleCode).IsUnique();
        builder.Property(x => x.ModuleName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description);
        builder.Property(x => x.Category).HasMaxLength(20).HasDefaultValue("core");
        builder.Property(x => x.FrappeAppName).HasMaxLength(100);
        builder.Property(x => x.GithubUrl).HasMaxLength(500);
        builder.Property(x => x.Version).HasMaxLength(20);
        builder.Property(x => x.Features);
        builder.Property(x => x.Doctypes);
        builder.Property(x => x.Dependencies);
        builder.Property(x => x.IsFree).HasDefaultValue(false);
        builder.Property(x => x.AddonPriceMonthly).HasPrecision(10, 2).HasDefaultValue(0);
        builder.Property(x => x.AddonPriceYearly).HasPrecision(10, 2).HasDefaultValue(0);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.IsBeta).HasDefaultValue(false);
        builder.Property(x => x.Icon).HasMaxLength(50);
        builder.Property(x => x.Color).HasMaxLength(20);
        builder.Property(x => x.DisplayOrder).HasDefaultValue(0);
        builder.HasIndex(x => x.Category);
        builder.HasIndex(x => x.IsActive);
    }
}

public static class ModuleConfigurationExtensions
{
    public static void ConfigureModule(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ModuleConfiguration());
    }
}
