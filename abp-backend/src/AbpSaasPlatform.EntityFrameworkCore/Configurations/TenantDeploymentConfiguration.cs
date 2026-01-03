using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Containers;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TenantDeploymentConfiguration : IEntityTypeConfiguration<TenantDeployment>
{
    public void Configure(EntityTypeBuilder<TenantDeployment> builder)
    {
        builder.ToTable("tenant_deployments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DeploymentId).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.DeploymentId).IsUnique();
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.DeploymentType).HasMaxLength(50).HasDefaultValue("initial");
        builder.Property(x => x.ImageId).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(50).HasDefaultValue("pending");
        builder.Property(x => x.StatusMessage).HasMaxLength(1000);
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
        builder.HasIndex(x => x.DeploymentId);
    }
}

public static class TenantDeploymentConfigurationExtensions
{
    public static void ConfigureTenantDeployment(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TenantDeploymentConfiguration());
    }
}
