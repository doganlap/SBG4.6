using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Admin;

public class AdminUser : AuditedEntity<int>
{
    public string UserId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    
    // Profile
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    
    // Role & Permissions
    public string Role { get; set; } = "admin"; // super_admin, admin, support, billing, readonly
    public string? Permissions { get; set; } // JSON
    
    // Security
    public bool IsActive { get; set; } = true;
    public bool IsVerified { get; set; } = false;
    public bool TwoFactorEnabled { get; set; } = false;
    public string? TwoFactorSecret { get; set; }
    
    // Session
    public DateTime? LastLoginAt { get; set; }
    public string? LastLoginIp { get; set; }
    public int FailedLoginAttempts { get; set; } = 0;
    public DateTime? LockedUntil { get; set; }
}
