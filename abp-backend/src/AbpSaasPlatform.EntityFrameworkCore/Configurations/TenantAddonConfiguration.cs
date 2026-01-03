using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Subscriptions;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TenantAddonConfiguration : IEntityTypeConfiguration<TenantAddon>
{
    public void Configure(EntityTypeBuilder<TenantAddon> builder)
    {
        builder.ToTable("tenant_addons");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.AddonId).IsRequired();
        builder.Property(x => x.SubscriptionId).IsRequired();
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.Quantity).HasDefaultValue(1);
        builder.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate);
        builder.Property(x => x.CancelledAt);
        builder.Property(x => x.UnitPrice).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.TotalPrice).HasPrecision(10, 2).IsRequired();
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Addon)
            .WithMany()
            .HasForeignKey(x => x.AddonId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Subscription)
            .WithMany()
            .HasForeignKey(x => x.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.AddonId);
        builder.HasIndex(x => x.Status);
    }
}

public static class TenantAddonConfigurationExtensions
{
    public static void ConfigureTenantAddon(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TenantAddonConfiguration());
    }
}
