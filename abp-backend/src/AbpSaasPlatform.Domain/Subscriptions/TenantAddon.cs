using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.Subscriptions;

public class TenantAddon : FullAuditedAggregateRoot<long>
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public int AddonId { get; set; }
    public SubscriptionAddon Addon { get; set; } = null!;
    
    public Guid SubscriptionId { get; set; }
    public Subscription Subscription { get; set; } = null!;
    
    // Quantity
    public int Quantity { get; set; } = 1;
    
    // Status
    public string Status { get; set; } = "active"; // active, cancelled, expired
    
    // Dates
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? CancelledAt { get; set; }
    
    // Pricing at time of purchase
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
