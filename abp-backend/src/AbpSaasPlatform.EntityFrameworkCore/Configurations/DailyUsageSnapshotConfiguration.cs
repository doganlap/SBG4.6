using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class DailyUsageSnapshotConfiguration : IEntityTypeConfiguration<DailyUsageSnapshot>
{
    public void Configure(EntityTypeBuilder<DailyUsageSnapshot> builder)
    {
        builder.ToTable("daily_usage_snapshots");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.SnapshotDate).IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.SnapshotDate }).IsUnique();
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.SnapshotDate);
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public static class DailyUsageSnapshotConfigurationExtensions
{
    public static void ConfigureDailyUsageSnapshot(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new DailyUsageSnapshotConfiguration());
    }
}
