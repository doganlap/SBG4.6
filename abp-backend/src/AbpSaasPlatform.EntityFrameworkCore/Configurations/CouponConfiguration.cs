using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("coupons");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CouponCode).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.CouponCode).IsUnique();
        builder.Property(x => x.CouponName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.DiscountType).HasMaxLength(50).HasDefaultValue("percentage");
        builder.Property(x => x.DiscountValue).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.Currency).HasMaxLength(3).HasDefaultValue("USD");
        builder.Property(x => x.MinimumAmount).HasPrecision(10, 2);
        builder.Property(x => x.MaximumDiscountAmount).HasPrecision(10, 2);
        builder.Property(x => x.CurrentRedemptions).HasDefaultValue(0);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.CouponCode);
    }
}

public static class CouponConfigurationExtensions
{
    public static void ConfigureCoupon(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CouponConfiguration());
    }
}
