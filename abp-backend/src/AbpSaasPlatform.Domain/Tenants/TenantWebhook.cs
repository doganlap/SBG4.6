using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Tenants;

public class TenantWebhook : FullAuditedAggregateRoot<long>
{
    public string WebhookId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public string Url { get; set; } = null!;
    public string HttpMethod { get; set; } = "POST"; // GET, POST, PUT, DELETE
    
    // Events
    public string Events { get; set; } = null!; // JSON array of event types
    
    // Security
    public string? Secret { get; set; }
    public string? Headers { get; set; } // JSON object
    
    // Status
    public bool IsActive { get; set; } = true;
    
    // Retry
    public int MaxRetries { get; set; } = 3;
    public int RetryDelaySeconds { get; set; } = 60;
    
    // Usage
    public DateTime? LastTriggeredAt { get; set; }
    public long SuccessCount { get; set; } = 0;
    public long FailureCount { get; set; } = 0;
    
    // Creator
    public Guid? CreatedBy { get; set; }
    public TenantUser? CreatedByUser { get; set; }
}
