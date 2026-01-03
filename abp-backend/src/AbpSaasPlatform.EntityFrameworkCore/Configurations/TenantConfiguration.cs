using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("tenants");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TenantId_Public).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.TenantId_Public).IsUnique();
        builder.Property(x => x.OrganizationName).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.Slug).IsUnique();
        builder.Property(x => x.Domain).HasMaxLength(255);
        builder.HasIndex(x => x.Domain).IsUnique();
        builder.Property(x => x.Subdomain).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.Subdomain).IsUnique();
        
        // Contact Information
        builder.Property(x => x.PrimaryEmail).HasMaxLength(255).IsRequired();
        builder.Property(x => x.PrimaryPhone).HasMaxLength(50);
        builder.Property(x => x.BillingEmail).HasMaxLength(255);
        
        // Address
        builder.Property(x => x.AddressLine1).HasMaxLength(255);
        builder.Property(x => x.AddressLine2).HasMaxLength(255);
        builder.Property(x => x.City).HasMaxLength(100);
        builder.Property(x => x.State).HasMaxLength(100);
        builder.Property(x => x.Country).HasMaxLength(100);
        builder.Property(x => x.PostalCode).HasMaxLength(20);
        
        // Business Information
        builder.Property(x => x.CompanyType).HasMaxLength(20).HasDefaultValue("company");
        builder.Property(x => x.Industry).HasMaxLength(100);
        builder.Property(x => x.TaxId).HasMaxLength(100);
        builder.Property(x => x.RegistrationNumber).HasMaxLength(100);
        
        // Status
        builder.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(x => x.ActivationDate);
        builder.Property(x => x.SuspensionDate);
        builder.Property(x => x.SuspensionReason);
        
        // Metadata
        builder.Property(x => x.LogoUrl).HasMaxLength(500);
        builder.Property(x => x.Timezone).HasMaxLength(100).HasDefaultValue("UTC");
        builder.Property(x => x.DefaultLanguage).HasMaxLength(10).HasDefaultValue("en");
        builder.Property(x => x.DefaultCurrency).HasMaxLength(3).HasDefaultValue("USD");
        
        // Technical
        builder.Property(x => x.DatabaseName).HasMaxLength(100);
        builder.HasIndex(x => x.DatabaseName).IsUnique();
        builder.Property(x => x.DatabaseHost).HasMaxLength(255);
        builder.Property(x => x.ContainerId).HasMaxLength(100);
        builder.Property(x => x.ErpnextSiteName).HasMaxLength(255);
        
        // Legacy fields
        builder.Ignore(x => x.Name);
        builder.Ignore(x => x.Plan);
        builder.Ignore(x => x.ActivatedAt);
        
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.OrganizationName);
    }
}

public static class TenantConfigurationExtensions
{
    public static void ConfigureTenant(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TenantConfiguration());
    }
}
