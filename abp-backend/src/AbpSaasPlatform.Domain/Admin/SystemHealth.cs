using Volo.Abp.Domain.Entities;

namespace AbpSaasPlatform.Admin;

public class SystemHealth : Entity<long>
{
    public string ServiceName { get; set; } = null!;
    public string? InstanceId { get; set; }
    
    // Status
    public string Status { get; set; } = "unknown"; // healthy, degraded, unhealthy, unknown
    
    // Metrics
    public decimal? CpuUsagePercent { get; set; }
    public decimal? MemoryUsagePercent { get; set; }
    public decimal? DiskUsagePercent { get; set; }
    public int? ResponseTimeMs { get; set; }
    
    // Details
    public string? HealthCheckUrl { get; set; }
    public string? LastError { get; set; }
    public string? Details { get; set; } // JSON
    
    // Timestamp
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
}
