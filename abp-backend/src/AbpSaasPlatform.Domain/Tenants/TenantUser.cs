using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Tenants;

public class TenantUser : FullAuditedAggregateRoot<Guid>
{
    public TenantUser(Guid id) : base(id)
    {
    }

    public string UserId { get; set; } = null!; // Public-facing ID
    
    // Authentication
    public string Email { get; set; } = null!;
    public string? PasswordHash { get; set; }
    
    // Profile
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? AvatarUrl { get; set; }
    
    // Employment
    public string? EmployeeId { get; set; }
    public string? Department { get; set; }
    public string? Designation { get; set; }
    public Guid? ReportsTo { get; set; }
    public TenantUser? ReportsToUser { get; set; }
    
    // Role & Permissions
    public int? RoleId { get; set; }
    public bool IsAdmin { get; set; } = false;
    public string? Permissions { get; set; } // JSON
    
    // Access
    public string? AllowedModules { get; set; } // JSON array
    
    // Security
    public bool IsActive { get; set; } = true;
    public bool IsVerified { get; set; } = false;
    public bool TwoFactorEnabled { get; set; } = false;
    public string? TwoFactorSecret { get; set; }
    
    // Session
    public DateTime? LastLoginAt { get; set; }
    public string? LastLoginIp { get; set; }
    public DateTime? LastActivityAt { get; set; }
    public int FailedLoginAttempts { get; set; } = 0;
    public DateTime? LockedUntil { get; set; }
    
    // ERPNext Integration
    public string? ErpnextUserId { get; set; }
    public string? ErpnextUserEmail { get; set; }
}
