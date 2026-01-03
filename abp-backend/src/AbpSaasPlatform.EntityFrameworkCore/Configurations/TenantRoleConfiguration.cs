using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TenantRoleConfiguration : IEntityTypeConfiguration<TenantRole>
{
    public void Configure(EntityTypeBuilder<TenantRole> builder)
    {
        builder.ToTable("tenant_roles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.RoleCode).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.RoleCode).IsUnique();
        builder.Property(x => x.RoleName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description);
        builder.Property(x => x.Permissions);
        builder.Property(x => x.AllowedModules);
        builder.Property(x => x.Level).HasDefaultValue(0);
        builder.Property(x => x.ParentRoleId);
        builder.Property(x => x.IsSystemRole).HasDefaultValue(false);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.HasOne(x => x.ParentRole)
            .WithMany()
            .HasForeignKey(x => x.ParentRoleId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(x => x.RoleCode);
        builder.HasIndex(x => x.Level);
    }
}

public static class TenantRoleConfigurationExtensions
{
    public static void ConfigureTenantRole(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TenantRoleConfiguration());
    }
}
