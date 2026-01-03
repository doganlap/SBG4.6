using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.Subscriptions;

public class Subscription : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid TenantId_Ref { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public string SubscriptionId { get; set; } = null!; // Public-facing ID like SUB001
    public int PlanId { get; set; }
    public SubscriptionPlan Plan { get; set; } = null!;
    
    // Status
    public string Status { get; set; } = "trial"; // trial, active, past_due, cancelled, paused, expired
    
    // Dates
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? TrialStartDate { get; set; }
    public DateTime? TrialEndDate { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
    
    // Billing
    public string BillingCycle { get; set; } = "monthly"; // monthly, yearly, custom
    public int BillingAnchorDay { get; set; } = 1; // Day of month for billing
    public DateTime? NextBillingDate { get; set; }
    
    // Pricing at time of subscription
    public decimal BasePrice { get; set; }
    public decimal DiscountPercent { get; set; } = 0;
    public decimal DiscountAmount { get; set; } = 0;
    public decimal FinalPrice { get; set; }
    public string Currency { get; set; } = "USD";
    
    // Usage
    public int CurrentUsers { get; set; } = 0;
    public decimal CurrentStorageGb { get; set; } = 0;
    public int CurrentApiCalls { get; set; } = 0;
    
    // External References
    public string? StripeSubscriptionId { get; set; }
    public string? StripeCustomerId { get; set; }
}
