using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class CouponRedemptionConfiguration : IEntityTypeConfiguration<CouponRedemption>
{
    public void Configure(EntityTypeBuilder<CouponRedemption> builder)
    {
        builder.ToTable("coupon_redemptions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CouponId).IsRequired();
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.DiscountAmount).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.RedeemedAt).IsRequired();
        builder.HasOne(x => x.Coupon)
            .WithMany()
            .HasForeignKey(x => x.CouponId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Invoice)
            .WithMany()
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(x => x.CouponId);
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.RedeemedAt);
    }
}

public static class CouponRedemptionConfigurationExtensions
{
    public static void ConfigureCouponRedemption(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CouponRedemptionConfiguration());
    }
}
