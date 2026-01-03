using Volo.Abp.Domain.Entities;
using AbpSaasPlatform.Containers;

namespace AbpSaasPlatform.Containers;

public class DeploymentLog : Entity<long>
{
    public long DeploymentId { get; set; }
    public TenantDeployment Deployment { get; set; } = null!;
    
    public string LogLevel { get; set; } = "info"; // debug, info, warning, error
    public string Message { get; set; } = null!;
    public string? Details { get; set; } // JSON
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
