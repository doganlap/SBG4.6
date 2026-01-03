using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TenantApiKeyConfiguration : IEntityTypeConfiguration<TenantApiKey>
{
    public void Configure(EntityTypeBuilder<TenantApiKey> builder)
    {
        builder.ToTable("tenant_api_keys");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.KeyId).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.KeyId).IsUnique();
        builder.Property(x => x.ApiKeyPrefix).HasMaxLength(10).IsRequired();
        builder.Property(x => x.ApiKeyHash).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.RateLimitPerMinute).HasDefaultValue(100);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.UsageCount).HasDefaultValue(0);
        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(x => x.KeyId);
    }
}

public static class TenantApiKeyConfigurationExtensions
{
    public static void ConfigureTenantApiKey(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TenantApiKeyConfiguration());
    }
}
