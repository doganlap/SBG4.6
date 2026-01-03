using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Tenants;
using AbpSaasPlatform.Billing;

namespace AbpSaasPlatform.Billing;

public class CouponRedemption : AuditedEntity<long>
{
    public int CouponId { get; set; }
    public Coupon Coupon { get; set; } = null!;
    
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public Guid? SubscriptionId { get; set; }
    public long? InvoiceId { get; set; }
    public BillingInvoice? Invoice { get; set; }
    
    public decimal DiscountAmount { get; set; }
    public DateTime RedeemedAt { get; set; } = DateTime.UtcNow;
    
    public string? RedeemedBy { get; set; } // User ID or email
}
