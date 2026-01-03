using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class BillingInvoiceConfiguration : IEntityTypeConfiguration<BillingInvoice>
{
    public void Configure(EntityTypeBuilder<BillingInvoice> builder)
    {
        builder.ToTable("invoices");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.InvoiceId).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.InvoiceId).IsUnique();
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.SubscriptionId);
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.InvoiceType).HasMaxLength(20).HasDefaultValue("subscription");
        builder.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("draft");
        builder.Property(x => x.InvoiceDate).IsRequired();
        builder.Property(x => x.DueDate).IsRequired();
        builder.Property(x => x.PaidAt);
        builder.Property(x => x.Subtotal).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.TaxAmount).HasPrecision(10, 2).HasDefaultValue(0);
        builder.Property(x => x.TaxPercent).HasPrecision(5, 2).HasDefaultValue(0);
        builder.Property(x => x.DiscountAmount).HasPrecision(10, 2).HasDefaultValue(0);
        builder.Property(x => x.TotalAmount).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.AmountPaid).HasPrecision(10, 2).HasDefaultValue(0);
        builder.Property(x => x.AmountDue).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.Currency).HasMaxLength(3).HasDefaultValue("USD");
        builder.Property(x => x.PeriodStart);
        builder.Property(x => x.PeriodEnd);
        builder.Property(x => x.LineItems);
        builder.Property(x => x.PaymentMethod).HasMaxLength(50);
        builder.Property(x => x.StripeInvoiceId).HasMaxLength(255);
        builder.Property(x => x.StripePaymentIntentId).HasMaxLength(255);
        builder.Property(x => x.PdfUrl).HasMaxLength(500);
        builder.Property(x => x.Notes);
        builder.Property(x => x.InternalNotes);
        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.InvoiceDate);
        builder.HasIndex(x => x.DueDate);
    }
}

public static class BillingInvoiceConfigurationExtensions
{
    public static void ConfigureBillingInvoice(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new BillingInvoiceConfiguration());
    }
}
