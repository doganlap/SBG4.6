using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Admin;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TargetType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.TargetId).HasMaxLength(50);
        builder.Property(x => x.Title).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Message).IsRequired();
        builder.Property(x => x.Type).HasMaxLength(50).HasDefaultValue("info");
        builder.Property(x => x.Category).HasMaxLength(50);
        builder.Property(x => x.ActionUrl).HasMaxLength(500);
        builder.Property(x => x.ActionText).HasMaxLength(100);
        builder.Property(x => x.IsRead).HasDefaultValue(false);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.HasIndex(x => new { x.TargetType, x.TargetId });
        builder.HasIndex(x => x.IsRead);
        builder.HasIndex(x => x.CreatedAt);
    }
}

public static class NotificationConfigurationExtensions
{
    public static void ConfigureNotification(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new NotificationConfiguration());
    }
}
