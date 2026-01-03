using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.Billing;

public class CreditTransaction : AuditedEntity<long>
{
    public long CreditId { get; set; }
    public Credit Credit { get; set; } = null!;
    
    public string TransactionType { get; set; } = null!; // applied, refunded, expired, adjusted
    public decimal Amount { get; set; }
    
    public long? InvoiceId { get; set; }
    public BillingInvoice? Invoice { get; set; }
    
    public string? Description { get; set; }
    public string? ReferenceId { get; set; }
    
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}
