using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Admin;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class AdminUserConfiguration : IEntityTypeConfiguration<AdminUser>
{
    public void Configure(EntityTypeBuilder<AdminUser> builder)
    {
        builder.ToTable("admin_users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.UserId).IsUnique();
        builder.Property(x => x.Email).HasMaxLength(255).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.PasswordHash).HasMaxLength(255).IsRequired();
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);
        builder.Property(x => x.Phone).HasMaxLength(50);
        builder.Property(x => x.AvatarUrl).HasMaxLength(500);
        builder.Property(x => x.Role).HasMaxLength(50).HasDefaultValue("admin");
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.IsVerified).HasDefaultValue(false);
        builder.Property(x => x.TwoFactorEnabled).HasDefaultValue(false);
        builder.Property(x => x.TwoFactorSecret).HasMaxLength(255);
        builder.Property(x => x.LastLoginIp).HasMaxLength(45);
        builder.Property(x => x.FailedLoginAttempts).HasDefaultValue(0);
        builder.HasIndex(x => x.Email);
        builder.HasIndex(x => x.Role);
        builder.HasIndex(x => x.IsActive);
    }
}

public static class AdminUserConfigurationExtensions
{
    public static void ConfigureAdminUser(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AdminUserConfiguration());
    }
}
