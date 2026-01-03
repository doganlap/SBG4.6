using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Containers;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TenantDatabaseConfiguration : IEntityTypeConfiguration<TenantDatabase>
{
    public void Configure(EntityTypeBuilder<TenantDatabase> builder)
    {
        builder.ToTable("tenant_databases");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.DatabaseName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.DatabaseHost).HasMaxLength(255).IsRequired();
        builder.Property(x => x.DatabasePort).HasDefaultValue(3306);
        builder.Property(x => x.DatabaseType).HasMaxLength(50).HasDefaultValue("mysql");
        builder.Property(x => x.DatabaseVersion).HasMaxLength(50);
        builder.Property(x => x.Status).HasMaxLength(50).HasDefaultValue("active");
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.DatabaseName);
    }
}

public static class TenantDatabaseConfigurationExtensions
{
    public static void ConfigureTenantDatabase(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TenantDatabaseConfiguration());
    }
}
