using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TaxRateConfiguration : IEntityTypeConfiguration<TaxRate>
{
    public void Configure(EntityTypeBuilder<TaxRate> builder)
    {
        builder.ToTable("tax_rates");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TaxCode).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.TaxCode).IsUnique();
        builder.Property(x => x.TaxName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Rate).HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.Country).HasMaxLength(100);
        builder.Property(x => x.State).HasMaxLength(100);
        builder.Property(x => x.City).HasMaxLength(100);
        builder.Property(x => x.TaxType).HasMaxLength(50).HasDefaultValue("sales");
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.Country);
        builder.HasIndex(x => x.State);
    }
}

public static class TaxRateConfigurationExtensions
{
    public static void ConfigureTaxRate(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TaxRateConfiguration());
    }
}
