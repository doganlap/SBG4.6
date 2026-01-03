using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Containers;

public class ContainerHost : AuditedEntity<int>
{
    public string HostId { get; set; } = null!;
    
    // Host info
    public string Hostname { get; set; } = null!;
    public string IpAddress { get; set; } = null!;
    
    // Type
    public string HostType { get; set; } = "docker"; // docker, kubernetes, swarm, ecs, other
    
    // Region/Zone
    public string? Region { get; set; }
    public string? AvailabilityZone { get; set; }
    
    // Capacity
    public int? TotalCpuCores { get; set; }
    public int? TotalMemoryGb { get; set; }
    public int? TotalStorageGb { get; set; }
    
    // Current usage
    public decimal? UsedCpuCores { get; set; }
    public decimal? UsedMemoryGb { get; set; }
    public decimal? UsedStorageGb { get; set; }
    
    // Container count
    public int MaxContainers { get; set; } = 100;
    public int CurrentContainers { get; set; } = 0;
    
    // Status
    public string Status { get; set; } = "active"; // active, draining, maintenance, offline
    
    // Metadata
    public string? Labels { get; set; } // JSON
    
    // Timestamps
    public DateTime? LastHeartbeat { get; set; }
}
