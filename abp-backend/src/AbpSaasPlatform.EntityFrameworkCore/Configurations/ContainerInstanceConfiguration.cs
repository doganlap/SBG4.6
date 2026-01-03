using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Containers;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class ContainerInstanceConfiguration : IEntityTypeConfiguration<ContainerInstance>
{
    public void Configure(EntityTypeBuilder<ContainerInstance> builder)
    {
        builder.ToTable("container_instances");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.InstanceId).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.InstanceId).IsUnique();
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.ImageId).IsRequired();
        builder.Property(x => x.ContainerId).HasMaxLength(100);
        builder.Property(x => x.ContainerName).HasMaxLength(255);
        builder.Property(x => x.InstanceType).HasMaxLength(30).IsRequired();
        builder.Property(x => x.HostId);
        builder.Property(x => x.HostIp).HasMaxLength(45);
        builder.Property(x => x.InternalPort);
        builder.Property(x => x.ExternalPort);
        builder.Property(x => x.CpuLimit).HasPrecision(5, 2);
        builder.Property(x => x.MemoryLimitMb);
        builder.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("creating");
        builder.Property(x => x.HealthStatus).HasMaxLength(20).HasDefaultValue("unknown");
        builder.Property(x => x.CpuUsagePercent).HasPrecision(5, 2);
        builder.Property(x => x.MemoryUsageMb);
        builder.Property(x => x.RestartCount).HasDefaultValue(0);
        builder.Property(x => x.StartedAt);
        builder.Property(x => x.StoppedAt);
        builder.Property(x => x.LastHealthCheck);
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Image)
            .WithMany()
            .HasForeignKey(x => x.ImageId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.InstanceType);
        builder.HasIndex(x => x.HealthStatus);
    }
}

public static class ContainerInstanceConfigurationExtensions
{
    public static void ConfigureContainerInstance(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ContainerInstanceConfiguration());
    }
}
