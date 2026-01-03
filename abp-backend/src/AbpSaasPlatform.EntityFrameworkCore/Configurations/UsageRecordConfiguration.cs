using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class UsageRecordConfiguration : IEntityTypeConfiguration<UsageRecord>
{
    public void Configure(EntityTypeBuilder<UsageRecord> builder)
    {
        builder.ToTable("usage_records");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.SubscriptionId).IsRequired();
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.UsagePeriodStart).IsRequired();
        builder.Property(x => x.UsagePeriodEnd).IsRequired();
        builder.Property(x => x.UserCount).HasDefaultValue(0);
        builder.Property(x => x.StorageUsedBytes).HasDefaultValue(0);
        builder.Property(x => x.ApiCalls).HasDefaultValue(0);
        builder.Property(x => x.BandwidthBytes).HasDefaultValue(0);
        builder.Property(x => x.ModuleUsage);
        builder.Property(x => x.OverageUserCost).HasPrecision(10, 2).HasDefaultValue(0);
        builder.Property(x => x.OverageStorageCost).HasPrecision(10, 2).HasDefaultValue(0);
        builder.Property(x => x.OverageApiCost).HasPrecision(10, 2).HasDefaultValue(0);
        builder.Property(x => x.TotalOverageCost).HasPrecision(10, 2).HasDefaultValue(0);
        builder.Property(x => x.IsBilled).HasDefaultValue(false);
        builder.Property(x => x.BilledInvoiceId);
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Subscription)
            .WithMany()
            .HasForeignKey(x => x.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.UsagePeriodStart, x.UsagePeriodEnd });
        builder.HasIndex(x => x.IsBilled);
        builder.HasIndex(x => new { x.TenantId, x.UsagePeriodStart, x.UsagePeriodEnd }).IsUnique();
    }
}

public static class UsageRecordConfigurationExtensions
{
    public static void ConfigureUsageRecord(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UsageRecordConfiguration());
    }
}
