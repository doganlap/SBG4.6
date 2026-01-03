using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Tenants;

public class TenantRole : AuditedEntity<int>
{
    public string RoleCode { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public string? Description { get; set; }
    
    // Permissions
    public string? Permissions { get; set; } // JSON - Detailed permission matrix
    public string? AllowedModules { get; set; } // JSON - Modules this role can access
    
    // Hierarchy
    public int Level { get; set; } = 0; // Higher = more access
    public int? ParentRoleId { get; set; }
    public TenantRole? ParentRole { get; set; }
    
    // Status
    public bool IsSystemRole { get; set; } = false; // Cannot be deleted
    public bool IsActive { get; set; } = true;
}
