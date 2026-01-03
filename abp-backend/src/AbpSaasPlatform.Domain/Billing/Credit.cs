using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.Billing;

public class Credit : FullAuditedAggregateRoot<long>
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public string CreditId { get; set; } = null!; // Public-facing ID
    public string CreditType { get; set; } = "manual"; // manual, refund, promotion, adjustment
    
    public decimal Amount { get; set; }
    public decimal Balance { get; set; } // Remaining balance
    public string Currency { get; set; } = "USD";
    
    public string? Description { get; set; }
    public string? Reason { get; set; }
    
    public DateTime? ExpiresAt { get; set; }
    public bool IsExpired { get; set; } = false;
    
    // Reference
    public long? RelatedInvoiceId { get; set; }
    public long? RelatedPaymentId { get; set; }
    
    // Metadata
    public string? Metadata { get; set; } // JSON
}
