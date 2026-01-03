using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Tenants;
using AbpSaasPlatform.Subscriptions;

namespace AbpSaasPlatform.Billing;

public class BillingInvoice : FullAuditedAggregateRoot<long>
{
    public string InvoiceId { get; set; } = null!; // e.g., INV-2024-001
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public Guid? SubscriptionId { get; set; }
    public Subscription? Subscription { get; set; }
    
    // Type
    public string InvoiceType { get; set; } = "subscription"; // subscription, addon, one_time, credit, refund
    
    // Status
    public string Status { get; set; } = "draft"; // draft, pending, paid, overdue, cancelled, refunded
    
    // Dates
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidAt { get; set; }
    
    // Amounts
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; } = 0;
    public decimal TaxPercent { get; set; } = 0;
    public decimal DiscountAmount { get; set; } = 0;
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; } = 0;
    public decimal AmountDue { get; set; }
    public string Currency { get; set; } = "USD";
    
    // Billing Period
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
    
    // Line Items (stored as JSON for simplicity)
    public string? LineItems { get; set; } // JSON array
    
    // Payment
    public string? PaymentMethod { get; set; }
    public string? StripeInvoiceId { get; set; }
    public string? StripePaymentIntentId { get; set; }
    
    // PDF
    public string? PdfUrl { get; set; }
    
    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }
}
