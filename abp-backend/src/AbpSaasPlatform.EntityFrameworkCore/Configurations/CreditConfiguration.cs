using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class CreditConfiguration : IEntityTypeConfiguration<Credit>
{
    public void Configure(EntityTypeBuilder<Credit> builder)
    {
        builder.ToTable("credits");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreditId).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.CreditId).IsUnique();
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.CreditType).HasMaxLength(50).HasDefaultValue("manual");
        builder.Property(x => x.Amount).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.Balance).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.Currency).HasMaxLength(3).HasDefaultValue("USD");
        builder.Property(x => x.IsExpired).HasDefaultValue(false);
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.IsExpired);
    }
}

public static class CreditConfigurationExtensions
{
    public static void ConfigureCredit(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CreditConfiguration());
    }
}
