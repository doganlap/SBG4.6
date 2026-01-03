using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TenantWebhookConfiguration : IEntityTypeConfiguration<TenantWebhook>
{
    public void Configure(EntityTypeBuilder<TenantWebhook> builder)
    {
        builder.ToTable("tenant_webhooks");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.WebhookId).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.WebhookId).IsUnique();
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Url).HasMaxLength(500).IsRequired();
        builder.Property(x => x.HttpMethod).HasMaxLength(10).HasDefaultValue("POST");
        builder.Property(x => x.Events).IsRequired();
        builder.Property(x => x.MaxRetries).HasDefaultValue(3);
        builder.Property(x => x.RetryDelaySeconds).HasDefaultValue(60);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.SuccessCount).HasDefaultValue(0);
        builder.Property(x => x.FailureCount).HasDefaultValue(0);
        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(x => x.WebhookId);
    }
}

public static class TenantWebhookConfigurationExtensions
{
    public static void ConfigureTenantWebhook(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TenantWebhookConfiguration());
    }
}
