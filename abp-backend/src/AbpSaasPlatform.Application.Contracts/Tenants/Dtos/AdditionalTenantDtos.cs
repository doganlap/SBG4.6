using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Tenants.Dtos;

public class TenantDepartmentDto : AuditedEntityDto<int>
{
    public string DepartmentCode { get; set; }
    public string DepartmentName { get; set; }
    public string? Description { get; set; }
    public int? ParentDepartmentId { get; set; }
    public Guid? HeadUserId { get; set; }
    public string? CostCenter { get; set; }
    public bool IsActive { get; set; }
}

public class TenantModuleSettingDto : AuditedEntityDto<int>
{
    public string ModuleCode { get; set; }
    public string? Settings { get; set; }
    public string? CustomFields { get; set; }
    public string? WorkflowConfig { get; set; }
    public string? EnabledFeatures { get; set; }
    public string? DisabledFeatures { get; set; }
    public string? DashboardConfig { get; set; }
    public string? ListViewConfig { get; set; }
    public string? FormLayoutConfig { get; set; }
}

public class TenantApiKeyDto : FullAuditedEntityDto<long>
{
    public string KeyId { get; set; }
    public string ApiKeyPrefix { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Scopes { get; set; }
    public string? AllowedIps { get; set; }
    public int RateLimitPerMinute { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public long UsageCount { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public Guid? CreatedBy { get; set; }
}

public class TenantWebhookDto : FullAuditedEntityDto<long>
{
    public string WebhookId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Url { get; set; }
    public string HttpMethod { get; set; }
    public string Events { get; set; }
    public bool IsActive { get; set; }
    public int MaxRetries { get; set; }
    public int RetryDelaySeconds { get; set; }
    public DateTime? LastTriggeredAt { get; set; }
    public long SuccessCount { get; set; }
    public long FailureCount { get; set; }
    public Guid? CreatedBy { get; set; }
}

public class TenantAuditLogDto : EntityDto<long>
{
    public string ActorType { get; set; }
    public Guid? ActorId { get; set; }
    public string? ActorEmail { get; set; }
    public string Action { get; set; }
    public string ResourceType { get; set; }
    public string? ResourceId { get; set; }
    public string? Description { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? Metadata { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? RequestId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class TenantActivityLogDto : EntityDto<long>
{
    public Guid? UserId { get; set; }
    public string ActivityType { get; set; }
    public string? ResourceType { get; set; }
    public string? ResourceId { get; set; }
    public string? Description { get; set; }
    public string? Metadata { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class TenantNotificationDto : EntityDto<long>
{
    public string TargetType { get; set; }
    public string? TargetId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string Type { get; set; }
    public string? Category { get; set; }
    public string? ActionUrl { get; set; }
    public string? ActionText { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public string? Metadata { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

public class TenantFileDto : FullAuditedEntityDto<long>
{
    public string FileId { get; set; }
    public string FileName { get; set; }
    public string? OriginalFileName { get; set; }
    public string FileType { get; set; }
    public string MimeType { get; set; }
    public long FileSizeBytes { get; set; }
    public string StorageType { get; set; }
    public string StoragePath { get; set; }
    public string? StorageUrl { get; set; }
    public string? Folder { get; set; }
    public string? Tags { get; set; }
    public string AccessLevel { get; set; }
    public string? SharedWith { get; set; }
    public string? Metadata { get; set; }
    public string? ThumbnailUrl { get; set; }
    public Guid? UploadedBy { get; set; }
}
