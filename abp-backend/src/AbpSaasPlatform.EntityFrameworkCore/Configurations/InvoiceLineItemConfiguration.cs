using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItem>
{
    public void Configure(EntityTypeBuilder<InvoiceLineItem> builder)
    {
        builder.ToTable("invoice_line_items");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.InvoiceId).IsRequired();
        builder.Property(x => x.ItemType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Quantity).HasPrecision(10, 2).HasDefaultValue(1);
        builder.Property(x => x.UnitPrice).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.DiscountPercent).HasPrecision(5, 2).HasDefaultValue(0);
        builder.Property(x => x.DiscountAmount).HasPrecision(10, 2).HasDefaultValue(0);
        builder.Property(x => x.TaxPercent).HasPrecision(5, 2).HasDefaultValue(0);
        builder.Property(x => x.TaxAmount).HasPrecision(10, 2).HasDefaultValue(0);
        builder.Property(x => x.TotalAmount).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.DisplayOrder).HasDefaultValue(0);
        builder.HasOne(x => x.Invoice)
            .WithMany()
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.InvoiceId);
    }
}

public static class InvoiceLineItemConfigurationExtensions
{
    public static void ConfigureInvoiceLineItem(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new InvoiceLineItemConfiguration());
    }
}
