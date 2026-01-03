using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Tenants;
using AbpSaasPlatform.Subscriptions;

namespace AbpSaasPlatform.Billing;

public class UsageRecord : FullAuditedAggregateRoot<long>
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public Guid SubscriptionId { get; set; }
    public Subscription Subscription { get; set; } = null!;
    
    // Period
    public DateTime UsagePeriodStart { get; set; }
    public DateTime UsagePeriodEnd { get; set; }
    
    // Usage Metrics
    public int UserCount { get; set; } = 0;
    public long StorageUsedBytes { get; set; } = 0;
    public int ApiCalls { get; set; } = 0;
    public long BandwidthBytes { get; set; } = 0;
    
    // Module-specific usage
    public string? ModuleUsage { get; set; } // JSON
    
    // Calculated costs (for metered billing)
    public decimal OverageUserCost { get; set; } = 0;
    public decimal OverageStorageCost { get; set; } = 0;
    public decimal OverageApiCost { get; set; } = 0;
    public decimal TotalOverageCost { get; set; } = 0;
    
    // Status
    public bool IsBilled { get; set; } = false;
    public long? BilledInvoiceId { get; set; }
}
