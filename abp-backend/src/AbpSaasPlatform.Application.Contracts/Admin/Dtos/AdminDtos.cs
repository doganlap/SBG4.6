using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Admin.Dtos;

public class AdminUserDto : AuditedEntityDto<int>
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    public string Role { get; set; }
    public string? Permissions { get; set; }
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? LastLoginIp { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTime? LockedUntil { get; set; }
}

public class AuditLogDto : EntityDto<long>
{
    public string ActorType { get; set; }
    public string? ActorId { get; set; }
    public string? ActorEmail { get; set; }
    public string Action { get; set; }
    public string ResourceType { get; set; }
    public string? ResourceId { get; set; }
    public string? Description { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? Metadata { get; set; }
    public Guid? TenantId { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? RequestId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SystemHealthDto : EntityDto<long>
{
    public string ServiceName { get; set; }
    public string? InstanceId { get; set; }
    public string Status { get; set; }
    public decimal? CpuUsagePercent { get; set; }
    public decimal? MemoryUsagePercent { get; set; }
    public decimal? DiskUsagePercent { get; set; }
    public int? ResponseTimeMs { get; set; }
    public string? HealthCheckUrl { get; set; }
    public string? LastError { get; set; }
    public string? Details { get; set; }
    public DateTime CheckedAt { get; set; }
}

public class NotificationDto : EntityDto<long>
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
