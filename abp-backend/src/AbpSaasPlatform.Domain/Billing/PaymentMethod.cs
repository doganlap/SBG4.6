using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.Billing;

public class PaymentMethod : FullAuditedAggregateRoot<long>
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public string MethodType { get; set; } = null!; // card, bank_account, paypal, other
    public string? Provider { get; set; } // stripe, paypal, manual
    
    // Card details (if applicable)
    public string? CardBrand { get; set; }
    public string? CardLastFour { get; set; }
    public int? CardExpMonth { get; set; }
    public int? CardExpYear { get; set; }
    public string? CardHolderName { get; set; }
    
    // Bank account details (if applicable)
    public string? BankName { get; set; }
    public string? AccountType { get; set; }
    public string? AccountLastFour { get; set; }
    
    // External references
    public string? StripePaymentMethodId { get; set; }
    public string? PaypalBillingAgreementId { get; set; }
    
    // Status
    public bool IsDefault { get; set; } = false;
    public bool IsActive { get; set; } = true;
    
    // Metadata
    public string? Metadata { get; set; } // JSON
}
