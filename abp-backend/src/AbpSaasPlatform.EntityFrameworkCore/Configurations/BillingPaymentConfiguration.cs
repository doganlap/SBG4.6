using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class BillingPaymentConfiguration : IEntityTypeConfiguration<BillingPayment>
{
    public void Configure(EntityTypeBuilder<BillingPayment> builder)
    {
        builder.ToTable("payments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PaymentId).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.PaymentId).IsUnique();
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.InvoiceId);
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.Amount).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.Currency).HasMaxLength(3).HasDefaultValue("USD");
        builder.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(x => x.PaymentMethod).HasMaxLength(20).HasDefaultValue("card");
        builder.Property(x => x.CardBrand).HasMaxLength(20);
        builder.Property(x => x.CardLastFour).HasMaxLength(4);
        builder.Property(x => x.StripePaymentId).HasMaxLength(255);
        builder.Property(x => x.StripeChargeId).HasMaxLength(255);
        builder.Property(x => x.PaypalTransactionId).HasMaxLength(255);
        builder.Property(x => x.Description);
        builder.Property(x => x.FailureReason);
        builder.Property(x => x.Metadata);
        builder.Property(x => x.PaymentDate).IsRequired();
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Invoice)
            .WithMany()
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.PaymentDate);
    }
}

public static class BillingPaymentConfigurationExtensions
{
    public static void ConfigureBillingPayment(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new BillingPaymentConfiguration());
    }
}
