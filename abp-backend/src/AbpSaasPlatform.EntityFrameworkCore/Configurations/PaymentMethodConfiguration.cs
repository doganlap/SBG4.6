using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("payment_methods");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.MethodType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Provider).HasMaxLength(50);
        builder.Property(x => x.CardBrand).HasMaxLength(50);
        builder.Property(x => x.CardLastFour).HasMaxLength(4);
        builder.Property(x => x.CardHolderName).HasMaxLength(100);
        builder.Property(x => x.BankName).HasMaxLength(100);
        builder.Property(x => x.AccountType).HasMaxLength(50);
        builder.Property(x => x.AccountLastFour).HasMaxLength(4);
        builder.Property(x => x.StripePaymentMethodId).HasMaxLength(255);
        builder.Property(x => x.PaypalBillingAgreementId).HasMaxLength(255);
        builder.Property(x => x.IsDefault).HasDefaultValue(false);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.TenantId);
    }
}

public static class PaymentMethodConfigurationExtensions
{
    public static void ConfigurePaymentMethod(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new PaymentMethodConfiguration());
    }
}
