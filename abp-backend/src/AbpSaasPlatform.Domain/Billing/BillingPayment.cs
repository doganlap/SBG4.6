using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Tenants;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.Billing;

public class BillingPayment : FullAuditedAggregateRoot<long>
{
    public string PaymentId { get; set; } = null!; // e.g., PAY-2024-001
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public long? InvoiceId { get; set; }
    public BillingInvoice? Invoice { get; set; }
    
    // Amount
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    
    // Status
    public string Status { get; set; } = "pending"; // pending, processing, succeeded, failed, refunded, cancelled
    
    // Payment Method
    public string PaymentMethod { get; set; } = "card"; // card, bank_transfer, paypal, manual, other
    public string? CardBrand { get; set; }
    public string? CardLastFour { get; set; }
    
    // External References
    public string? StripePaymentId { get; set; }
    public string? StripeChargeId { get; set; }
    public string? PaypalTransactionId { get; set; }
    
    // Metadata
    public string? Description { get; set; }
    public string? FailureReason { get; set; }
    public string? Metadata { get; set; } // JSON
    
    // Timestamps
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
}
