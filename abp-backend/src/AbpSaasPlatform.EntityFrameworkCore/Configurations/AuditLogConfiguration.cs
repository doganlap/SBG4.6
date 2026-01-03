using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Admin;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ActorType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.ActorId).HasMaxLength(50);
        builder.Property(x => x.ActorEmail).HasMaxLength(255);
        builder.Property(x => x.Action).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ResourceType).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ResourceId).HasMaxLength(100);
        builder.Property(x => x.IpAddress).HasMaxLength(45);
        builder.Property(x => x.RequestId).HasMaxLength(100);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.HasIndex(x => new { x.ActorType, x.ActorId });
        builder.HasIndex(x => x.Action);
        builder.HasIndex(x => new { x.ResourceType, x.ResourceId });
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.CreatedAt);
    }
}

public static class AuditLogConfigurationExtensions
{
    public static void ConfigureAuditLog(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AuditLogConfiguration());
    }
}
