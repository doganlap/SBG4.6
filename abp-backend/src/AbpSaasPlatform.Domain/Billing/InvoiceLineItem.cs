using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.Billing;

public class InvoiceLineItem : AuditedEntity<long>
{
    public long InvoiceId { get; set; }
    public BillingInvoice Invoice { get; set; } = null!;
    
    // Item details
    public string ItemType { get; set; } = null!; // subscription, addon, overage, credit, discount, tax
    public string Description { get; set; } = null!;
    
    // Pricing
    public decimal Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; } = 0;
    public decimal DiscountAmount { get; set; } = 0;
    public decimal TaxPercent { get; set; } = 0;
    public decimal TaxAmount { get; set; } = 0;
    public decimal TotalAmount { get; set; }
    
    // Reference
    public string? ReferenceType { get; set; } // subscription, addon, usage_record
    public string? ReferenceId { get; set; }
    
    // Period (for subscription/addon items)
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
    
    // Display
    public int DisplayOrder { get; set; } = 0;
}
