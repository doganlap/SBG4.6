using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TenantActivityLogConfiguration : IEntityTypeConfiguration<TenantActivityLog>
{
    public void Configure(EntityTypeBuilder<TenantActivityLog> builder)
    {
        builder.ToTable("tenant_activity_log");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ActivityType).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ResourceType).HasMaxLength(100);
        builder.Property(x => x.ResourceId).HasMaxLength(100);
        builder.Property(x => x.IpAddress).HasMaxLength(45);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.ActivityType);
        builder.HasIndex(x => x.CreatedAt);
    }
}

public static class TenantActivityLogConfigurationExtensions
{
    public static void ConfigureTenantActivityLog(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TenantActivityLogConfiguration());
    }
}
