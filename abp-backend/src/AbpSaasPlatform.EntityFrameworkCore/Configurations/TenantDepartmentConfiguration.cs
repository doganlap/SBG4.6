using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TenantDepartmentConfiguration : IEntityTypeConfiguration<TenantDepartment>
{
    public void Configure(EntityTypeBuilder<TenantDepartment> builder)
    {
        builder.ToTable("tenant_departments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DepartmentCode).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.DepartmentCode).IsUnique();
        builder.Property(x => x.DepartmentName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.CostCenter).HasMaxLength(100);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.HasOne(x => x.ParentDepartment)
            .WithMany()
            .HasForeignKey(x => x.ParentDepartmentId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(x => x.HeadUser)
            .WithMany()
            .HasForeignKey(x => x.HeadUserId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(x => x.DepartmentCode);
    }
}

public static class TenantDepartmentConfigurationExtensions
{
    public static void ConfigureTenantDepartment(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TenantDepartmentConfiguration());
    }
}
