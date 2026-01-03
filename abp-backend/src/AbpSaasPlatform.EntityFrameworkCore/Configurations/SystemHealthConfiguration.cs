using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Admin;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class SystemHealthConfiguration : IEntityTypeConfiguration<SystemHealth>
{
    public void Configure(EntityTypeBuilder<SystemHealth> builder)
    {
        builder.ToTable("system_health");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ServiceName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.InstanceId).HasMaxLength(100);
        builder.Property(x => x.Status).HasMaxLength(50).HasDefaultValue("unknown");
        builder.Property(x => x.CpuUsagePercent).HasPrecision(5, 2);
        builder.Property(x => x.MemoryUsagePercent).HasPrecision(5, 2);
        builder.Property(x => x.DiskUsagePercent).HasPrecision(5, 2);
        builder.Property(x => x.HealthCheckUrl).HasMaxLength(255);
        builder.Property(x => x.CheckedAt).IsRequired();
        builder.HasIndex(x => x.ServiceName);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.CheckedAt);
    }
}

public static class SystemHealthConfigurationExtensions
{
    public static void ConfigureSystemHealth(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new SystemHealthConfiguration());
    }
}
