using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Containers;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class ContainerHostConfiguration : IEntityTypeConfiguration<ContainerHost>
{
    public void Configure(EntityTypeBuilder<ContainerHost> builder)
    {
        builder.ToTable("container_hosts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.HostId).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.HostId).IsUnique();
        builder.Property(x => x.Hostname).HasMaxLength(255).IsRequired();
        builder.Property(x => x.IpAddress).HasMaxLength(45).IsRequired();
        builder.Property(x => x.HostType).HasMaxLength(20).HasDefaultValue("docker");
        builder.Property(x => x.Region).HasMaxLength(50);
        builder.Property(x => x.AvailabilityZone).HasMaxLength(50);
        builder.Property(x => x.TotalCpuCores);
        builder.Property(x => x.TotalMemoryGb);
        builder.Property(x => x.TotalStorageGb);
        builder.Property(x => x.UsedCpuCores).HasPrecision(5, 2);
        builder.Property(x => x.UsedMemoryGb).HasPrecision(10, 2);
        builder.Property(x => x.UsedStorageGb).HasPrecision(10, 2);
        builder.Property(x => x.MaxContainers).HasDefaultValue(100);
        builder.Property(x => x.CurrentContainers).HasDefaultValue(0);
        builder.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(x => x.Labels);
        builder.Property(x => x.LastHeartbeat);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.Region);
    }
}

public static class ContainerHostConfigurationExtensions
{
    public static void ConfigureContainerHost(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ContainerHostConfiguration());
    }
}
