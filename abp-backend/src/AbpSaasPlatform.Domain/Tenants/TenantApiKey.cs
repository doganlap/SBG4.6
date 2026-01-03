using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Tenants;

public class TenantApiKey : FullAuditedAggregateRoot<long>
{
    public string KeyId { get; set; } = null!;
    
    // Key
    public string ApiKeyPrefix { get; set; } = null!;
    public string ApiKeyHash { get; set; } = null!;
    
    // Metadata
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    // Permissions
    public string? Scopes { get; set; } // JSON array
    public string? AllowedIps { get; set; } // JSON array
    public int RateLimitPerMinute { get; set; } = 100;
    
    // Status
    public bool IsActive { get; set; } = true;
    
    // Usage
    public DateTime? LastUsedAt { get; set; }
    public long UsageCount { get; set; } = 0;
    
    // Expiry
    public DateTime? ExpiresAt { get; set; }
    
    // Creator
    public Guid? CreatedBy { get; set; }
    public TenantUser? CreatedByUser { get; set; }
}
