using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Containers;

namespace AbpSaasPlatform.Containers;

public class DatabaseBackup : FullAuditedAggregateRoot<long>
{
    public long DatabaseId { get; set; }
    public TenantDatabase Database { get; set; } = null!;
    
    public string BackupId { get; set; } = null!;
    public string BackupType { get; set; } = "full"; // full, incremental, differential
    
    public string StorageType { get; set; } = "local"; // local, s3, azure, gcs
    public string StoragePath { get; set; } = null!;
    public string? StorageUrl { get; set; }
    
    public long? SizeBytes { get; set; }
    public string Status { get; set; } = "pending"; // pending, in_progress, completed, failed
    
    public DateTime BackupDate { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    
    public string? ErrorMessage { get; set; }
    public string? Metadata { get; set; } // JSON
}
