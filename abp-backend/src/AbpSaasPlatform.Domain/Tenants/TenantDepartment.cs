using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Tenants;

public class TenantDepartment : AuditedEntity<int>
{
    public string DepartmentCode { get; set; } = null!;
    public string DepartmentName { get; set; } = null!;
    public string? Description { get; set; }
    
    // Hierarchy
    public int? ParentDepartmentId { get; set; }
    public TenantDepartment? ParentDepartment { get; set; }
    
    // Head
    public Guid? HeadUserId { get; set; }
    public TenantUser? HeadUser { get; set; }
    
    // Cost Center
    public string? CostCenter { get; set; }
    
    // Status
    public bool IsActive { get; set; } = true;
}
