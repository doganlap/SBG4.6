using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Tenants;
using AbpSaasPlatform.Containers;

namespace AbpSaasPlatform.Containers;

public class ContainerInstance : FullAuditedAggregateRoot<long>
{
    public string InstanceId { get; set; } = null!;
    
    // Tenant
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    // Image
    public int ImageId { get; set; }
    public ContainerImage Image { get; set; } = null!;
    
    // Container info
    public string? ContainerId { get; set; } // Docker container ID
    public string? ContainerName { get; set; }
    
    // Type
    public string InstanceType { get; set; } = null!; // backend, worker_short, worker_long, worker_default, scheduler, socketio
    
    // Host
    public int? HostId { get; set; }
    public string? HostIp { get; set; }
    
    // Ports
    public int? InternalPort { get; set; }
    public int? ExternalPort { get; set; }
    
    // Resources
    public decimal? CpuLimit { get; set; } // CPU cores limit
    public int? MemoryLimitMb { get; set; } // Memory limit in MB
    
    // Status
    public string Status { get; set; } = "creating"; // creating, running, stopped, failed, terminated
    public string HealthStatus { get; set; } = "unknown"; // healthy, unhealthy, unknown
    
    // Metrics
    public decimal? CpuUsagePercent { get; set; }
    public int? MemoryUsageMb { get; set; }
    public int RestartCount { get; set; } = 0;
    
    // Timestamps
    public DateTime? StartedAt { get; set; }
    public DateTime? StoppedAt { get; set; }
    public DateTime? LastHealthCheck { get; set; }
}
