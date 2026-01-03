using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.Containers;

public class TenantDeployment : FullAuditedAggregateRoot<long>
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public string DeploymentId { get; set; } = null!;
    public string DeploymentType { get; set; } = "initial"; // initial, update, rollback, maintenance
    
    public int ImageId { get; set; }
    public ContainerImage Image { get; set; } = null!;
    
    public string Status { get; set; } = "pending"; // pending, in_progress, completed, failed, cancelled
    public string? StatusMessage { get; set; }
    
    public int? ProgressPercent { get; set; }
    
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    public string? DeploymentConfig { get; set; } // JSON
    public string? ErrorDetails { get; set; }
}
