using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TenantUserConfiguration : IEntityTypeConfiguration<TenantUser>
{
    public void Configure(EntityTypeBuilder<TenantUser> builder)
    {
        builder.ToTable("tenant_users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.UserId).IsUnique();
        builder.Property(x => x.Email).HasMaxLength(255).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.PasswordHash).HasMaxLength(255);
        builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(50);
        builder.Property(x => x.Mobile).HasMaxLength(50);
        builder.Property(x => x.AvatarUrl).HasMaxLength(500);
        builder.Property(x => x.EmployeeId).HasMaxLength(50);
        builder.Property(x => x.Department).HasMaxLength(100);
        builder.Property(x => x.Designation).HasMaxLength(100);
        builder.Property(x => x.ReportsTo);
        builder.Property(x => x.RoleId);
        builder.Property(x => x.IsAdmin).HasDefaultValue(false);
        builder.Property(x => x.Permissions);
        builder.Property(x => x.AllowedModules);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.IsVerified).HasDefaultValue(false);
        builder.Property(x => x.TwoFactorEnabled).HasDefaultValue(false);
        builder.Property(x => x.TwoFactorSecret).HasMaxLength(255);
        builder.Property(x => x.LastLoginAt);
        builder.Property(x => x.LastLoginIp).HasMaxLength(45);
        builder.Property(x => x.LastActivityAt);
        builder.Property(x => x.FailedLoginAttempts).HasDefaultValue(0);
        builder.Property(x => x.LockedUntil);
        builder.Property(x => x.ErpnextUserId).HasMaxLength(100);
        builder.Property(x => x.ErpnextUserEmail).HasMaxLength(255);
        builder.HasOne(x => x.ReportsToUser)
            .WithMany()
            .HasForeignKey(x => x.ReportsTo)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(x => x.Email);
        builder.HasIndex(x => x.Department);
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.EmployeeId);
    }
}

public static class TenantUserConfigurationExtensions
{
    public static void ConfigureTenantUser(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TenantUserConfiguration());
    }
}
