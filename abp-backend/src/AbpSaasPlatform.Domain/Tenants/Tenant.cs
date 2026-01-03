using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace AbpSaasPlatform.Tenants;

public class Tenant : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Tenant(Guid id) : base(id)
    {
    }

    public Guid? TenantId { get; set; }
    public string TenantId_Public { get; set; } = null!; // Public-facing ID like TNT001
    public string OrganizationName { get; set; } = null!;
    public string Slug { get; set; } = null!; // URL-safe identifier
    public string? Domain { get; set; } // Custom domain if any
    public string Subdomain { get; set; } = null!; // e.g., acme.erp.localhost
    
    // Contact Information
    public string PrimaryEmail { get; set; } = null!;
    public string? PrimaryPhone { get; set; }
    public string? BillingEmail { get; set; }
    
    // Address
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    
    // Business Information
    public string CompanyType { get; set; } = "company"; // company, individual, non_profit, government, education
    public string? Industry { get; set; }
    public string? TaxId { get; set; }
    public string? RegistrationNumber { get; set; }
    
    // Status
    public string Status { get; set; } = "pending"; // pending, active, suspended, cancelled, trial, trial_expired
    public DateTime? ActivationDate { get; set; }
    public DateTime? SuspensionDate { get; set; }
    public string? SuspensionReason { get; set; }
    
    // Metadata
    public string? LogoUrl { get; set; }
    public string Timezone { get; set; } = "UTC";
    public string DefaultLanguage { get; set; } = "en";
    public string DefaultCurrency { get; set; } = "USD";
    
    // Technical
    public string? DatabaseName { get; set; }
    public string? DatabaseHost { get; set; }
    public string? ContainerId { get; set; }
    public string? ErpnextSiteName { get; set; }
    
    // Legacy fields for backward compatibility
    public string Name { get => OrganizationName; set => OrganizationName = value; }
    public string Plan { get; set; } = "Professional";
    public DateTime? ActivatedAt { get => ActivationDate; set => ActivationDate = value; }
    public long MaxUsers { get; set; } = 10;
    public long MaxStorage { get; set; } = 5368709120; // 5GB
}
