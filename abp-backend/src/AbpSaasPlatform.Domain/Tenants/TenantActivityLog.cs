using Volo.Abp.Domain.Entities;

namespace AbpSaasPlatform.Tenants;

public class TenantActivityLog : Entity<long>
{
    public Guid? UserId { get; set; }
    public TenantUser? User { get; set; }
    
    public string ActivityType { get; set; } = null!; // login, document_create, document_update, etc.
    public string? ResourceType { get; set; }
    public string? ResourceId { get; set; }
    
    public string? Description { get; set; }
    public string? Metadata { get; set; } // JSON
    
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
