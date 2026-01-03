using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Subscriptions;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class SubscriptionAddonConfiguration : IEntityTypeConfiguration<SubscriptionAddon>
{
    public void Configure(EntityTypeBuilder<SubscriptionAddon> builder)
    {
        builder.ToTable("subscription_addons");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AddonCode).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.AddonCode).IsUnique();
        builder.Property(x => x.AddonName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description);
        builder.Property(x => x.AddonType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.PriceMonthly).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.PriceYearly).HasPrecision(10, 2);
        builder.Property(x => x.Currency).HasMaxLength(3).HasDefaultValue("USD");
        builder.Property(x => x.IsQuantityBased).HasDefaultValue(false);
        builder.Property(x => x.QuantityUnit).HasMaxLength(50);
        builder.Property(x => x.MinQuantity).HasDefaultValue(1);
        builder.Property(x => x.MaxQuantity);
        builder.Property(x => x.ModuleId);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.HasOne(x => x.Module)
            .WithMany()
            .HasForeignKey(x => x.ModuleId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(x => x.AddonType);
        builder.HasIndex(x => x.IsActive);
    }
}

public static class SubscriptionAddonConfigurationExtensions
{
    public static void ConfigureSubscriptionAddon(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new SubscriptionAddonConfiguration());
    }
}
