using Volo.Abp.Domain.Entities;

namespace AbpSaasPlatform.Tenants;

public class TenantNotification : Entity<long>
{
    // Target
    public string TargetType { get; set; } = null!; // all_users, user, role, department
    public string? TargetId { get; set; }
    
    // Content
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string Type { get; set; } = "info"; // info, warning, error, success
    public string? Category { get; set; }
    
    // Action
    public string? ActionUrl { get; set; }
    public string? ActionText { get; set; }
    
    // Status
    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }
    
    // Metadata
    public string? Metadata { get; set; } // JSON
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }
}
