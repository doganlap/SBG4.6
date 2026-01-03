using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Containers.Dtos;

public class TenantDeploymentDto : FullAuditedEntityDto<long>
{
    public string DeploymentId { get; set; }
    public Guid TenantId { get; set; }
    public string DeploymentType { get; set; }
    public int ImageId { get; set; }
    public string Status { get; set; }
    public string? StatusMessage { get; set; }
    public int? ProgressPercent { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? DeploymentConfig { get; set; }
    public string? ErrorDetails { get; set; }
}

public class TenantDatabaseDto : FullAuditedEntityDto<long>
{
    public Guid TenantId { get; set; }
    public string DatabaseName { get; set; }
    public string DatabaseHost { get; set; }
    public int DatabasePort { get; set; }
    public string DatabaseType { get; set; }
    public string? DatabaseVersion { get; set; }
    public string Status { get; set; }
    public long? SizeBytes { get; set; }
    public int? TableCount { get; set; }
    public DateTime? LastBackupAt { get; set; }
    public DateTime? LastRestoredAt { get; set; }
    public string? Metadata { get; set; }
}

public class DatabaseBackupDto : FullAuditedEntityDto<long>
{
    public string BackupId { get; set; }
    public long DatabaseId { get; set; }
    public string BackupType { get; set; }
    public string StorageType { get; set; }
    public string StoragePath { get; set; }
    public string? StorageUrl { get; set; }
    public long? SizeBytes { get; set; }
    public string Status { get; set; }
    public DateTime BackupDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Metadata { get; set; }
}

public class DeploymentLogDto : EntityDto<long>
{
    public long DeploymentId { get; set; }
    public string LogLevel { get; set; }
    public string Message { get; set; }
    public string? Details { get; set; }
    public DateTime CreatedAt { get; set; }
}
