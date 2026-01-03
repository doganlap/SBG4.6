using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.Billing;

public class DailyUsageSnapshot : AuditedEntity<long>
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public DateTime SnapshotDate { get; set; }
    
    // User metrics
    public int TotalUsers { get; set; } = 0;
    public int ActiveUsers { get; set; } = 0;
    public int NewUsers { get; set; } = 0;
    
    // Storage metrics
    public long TotalStorageBytes { get; set; } = 0;
    public long StorageDeltaBytes { get; set; } = 0;
    
    // API metrics
    public int ApiCalls { get; set; } = 0;
    public int ApiErrors { get; set; } = 0;
    
    // Activity metrics
    public int DocumentsCreated { get; set; } = 0;
    public int DocumentsUpdated { get; set; } = 0;
    public int LoginCount { get; set; } = 0;
    
    // Performance
    public int? AvgResponseTimeMs { get; set; }
}
