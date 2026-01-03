using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Containers;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class DeploymentLogConfiguration : IEntityTypeConfiguration<DeploymentLog>
{
    public void Configure(EntityTypeBuilder<DeploymentLog> builder)
    {
        builder.ToTable("deployment_logs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DeploymentId).IsRequired();
        builder.Property(x => x.LogLevel).HasMaxLength(50).HasDefaultValue("info");
        builder.Property(x => x.Message).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.HasOne(x => x.Deployment)
            .WithMany()
            .HasForeignKey(x => x.DeploymentId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.DeploymentId);
        builder.HasIndex(x => x.CreatedAt);
    }
}

public static class DeploymentLogConfigurationExtensions
{
    public static void ConfigureDeploymentLog(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new DeploymentLogConfiguration());
    }
}
