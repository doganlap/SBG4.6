using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Subscriptions;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.ToTable("subscription_plans");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PlanCode).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.PlanCode).IsUnique();
        builder.Property(x => x.PlanName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description);
        builder.Property(x => x.PriceMonthly).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.PriceYearly).HasPrecision(10, 2);
        builder.Property(x => x.Currency).HasMaxLength(3).HasDefaultValue("USD");
        builder.Property(x => x.MaxUsers).HasDefaultValue(5);
        builder.Property(x => x.MaxStorageGb).HasDefaultValue(10);
        builder.Property(x => x.MaxApiCallsPerMonth).HasDefaultValue(10000);
        builder.Property(x => x.IncludedModules);
        builder.Property(x => x.Features);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.IsPublic).HasDefaultValue(true);
        builder.Property(x => x.IsTrialAvailable).HasDefaultValue(true);
        builder.Property(x => x.TrialDays).HasDefaultValue(14);
        builder.Property(x => x.DisplayOrder).HasDefaultValue(0);
        builder.Property(x => x.BadgeText).HasMaxLength(50);
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.IsPublic);
    }
}

public static class SubscriptionPlanConfigurationExtensions
{
    public static void ConfigureSubscriptionPlan(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new SubscriptionPlanConfiguration());
    }
}
