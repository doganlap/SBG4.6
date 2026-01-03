using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class CreditTransactionConfiguration : IEntityTypeConfiguration<CreditTransaction>
{
    public void Configure(EntityTypeBuilder<CreditTransaction> builder)
    {
        builder.ToTable("credit_transactions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreditId).IsRequired();
        builder.Property(x => x.TransactionType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Amount).HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.TransactionDate).IsRequired();
        builder.HasOne(x => x.Credit)
            .WithMany()
            .HasForeignKey(x => x.CreditId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Invoice)
            .WithMany()
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(x => x.CreditId);
        builder.HasIndex(x => x.TransactionDate);
    }
}

public static class CreditTransactionConfigurationExtensions
{
    public static void ConfigureCreditTransaction(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CreditTransactionConfiguration());
    }
}
