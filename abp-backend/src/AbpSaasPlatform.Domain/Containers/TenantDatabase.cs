using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.Containers;

public class TenantDatabase : FullAuditedAggregateRoot<long>
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public string DatabaseName { get; set; } = null!;
    public string DatabaseHost { get; set; } = null!;
    public int DatabasePort { get; set; } = 3306;
    
    public string DatabaseType { get; set; } = "mysql"; // mysql, postgresql, mariadb
    public string? DatabaseVersion { get; set; }
    
    public string Status { get; set; } = "active"; // active, backup, restored, deleted
    
    public long? SizeBytes { get; set; }
    public int? TableCount { get; set; }
    
    public DateTime? LastBackupAt { get; set; }
    public DateTime? LastRestoredAt { get; set; }
    
    public string? ConnectionString { get; set; } // Encrypted
    public string? Metadata { get; set; } // JSON
}
