using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Subscriptions;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("subscriptions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.SubscriptionId).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.SubscriptionId).IsUnique();
        builder.Property(x => x.TenantId_Ref).IsRequired();
        builder.Property(x => x.PlanId).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("trial");
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate);
        builder.Property(x => x.TrialStartDate);
        builder.Property(x => x.TrialEndDate);
        builder.Property(x => x.CancelledAt);
        builder.Property(x => x.CancellationReason);
        builder.Property(x => x.BillingCycle).HasMaxLength(20).HasDefaultValue("monthly");
        builder.Property(x => x.BillingAnchorDay).HasDefaultValue(1);
        builder.Property(x => x.NextBillingDate);
        builder.Property(x => x.BasePrice).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.DiscountPercent).HasPrecision(5, 2).HasDefaultValue(0);
        builder.Property(x => x.DiscountAmount).HasPrecision(10, 2).HasDefaultValue(0);
        builder.Property(x => x.FinalPrice).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.Currency).HasMaxLength(3).HasDefaultValue("USD");
        builder.Property(x => x.CurrentUsers).HasDefaultValue(0);
        builder.Property(x => x.CurrentStorageGb).HasPrecision(10, 2).HasDefaultValue(0);
        builder.Property(x => x.CurrentApiCalls).HasDefaultValue(0);
        builder.Property(x => x.StripeSubscriptionId).HasMaxLength(255);
        builder.Property(x => x.StripeCustomerId).HasMaxLength(255);
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId_Ref)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Plan)
            .WithMany()
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.TenantId_Ref);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.NextBillingDate);
    }
}

public static class SubscriptionConfigurationExtensions
{
    public static void ConfigureSubscription(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new SubscriptionConfiguration());
    }
}
