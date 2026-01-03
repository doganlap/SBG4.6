using Volo.Abp.Domain.Entities;

namespace AbpSaasPlatform.Tenants;

public class TenantAuditLog : Entity<long>
{
    // Actor
    public string ActorType { get; set; } = null!; // user, system, api
    public Guid? ActorId { get; set; }
    public string? ActorEmail { get; set; }
    
    // Action
    public string Action { get; set; } = null!;
    public string ResourceType { get; set; } = null!;
    public string? ResourceId { get; set; }
    
    // Details
    public string? Description { get; set; }
    public string? OldValues { get; set; } // JSON
    public string? NewValues { get; set; } // JSON
    public string? Metadata { get; set; } // JSON
    
    // Context
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? RequestId { get; set; }
    
    // Timestamp
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
