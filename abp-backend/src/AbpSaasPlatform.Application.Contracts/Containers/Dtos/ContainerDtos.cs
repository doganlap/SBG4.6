using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Containers.Dtos;

public class CreateContainerImageDto
{
    public string ImageId { get; set; }
    public string ImageName { get; set; }
    public string ImageTag { get; set; }
    public string FullImageUrl { get; set; }
    public string? RegistryUrl { get; set; }
    public string RegistryType { get; set; } = "dockerhub";
    public string ImageType { get; set; }
    public int? ModuleId { get; set; }
    public string? ErpnextVersion { get; set; }
    public string? FrappeVersion { get; set; }
    public int? ImageSizeMb { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsLatest { get; set; } = false;
    public DateTime? BuildDate { get; set; }
    public string? CommitHash { get; set; }
}

public class UpdateContainerImageDto
{
    public string? ImageTag { get; set; }
    public string? FullImageUrl { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsLatest { get; set; }
    public DateTime? BuildDate { get; set; }
    public string? CommitHash { get; set; }
}

public class ContainerImageDto : EntityDto<int>
{
    public string ImageId { get; set; }
    public string ImageName { get; set; }
    public string ImageTag { get; set; }
    public string FullImageUrl { get; set; }
    public string? RegistryUrl { get; set; }
    public string RegistryType { get; set; }
    public string ImageType { get; set; }
    public int? ModuleId { get; set; }
    public string? ErpnextVersion { get; set; }
    public string? FrappeVersion { get; set; }
    public int? ImageSizeMb { get; set; }
    public bool IsActive { get; set; }
    public bool IsLatest { get; set; }
    public DateTime? BuildDate { get; set; }
    public string? CommitHash { get; set; }
}

public class CreateContainerInstanceDto
{
    public Guid TenantId { get; set; }
    public int ImageId { get; set; }
    public string? ContainerName { get; set; }
    public string InstanceType { get; set; }
    public int? HostId { get; set; }
    public int? InternalPort { get; set; }
    public int? ExternalPort { get; set; }
    public decimal? CpuLimit { get; set; }
    public int? MemoryLimitMb { get; set; }
}

public class ContainerInstanceDto : EntityDto<long>
{
    public string InstanceId { get; set; }
    public Guid TenantId { get; set; }
    public int ImageId { get; set; }
    public string? ContainerId { get; set; }
    public string? ContainerName { get; set; }
    public string InstanceType { get; set; }
    public int? HostId { get; set; }
    public string? HostIp { get; set; }
    public int? InternalPort { get; set; }
    public int? ExternalPort { get; set; }
    public decimal? CpuLimit { get; set; }
    public int? MemoryLimitMb { get; set; }
    public string Status { get; set; }
    public string HealthStatus { get; set; }
    public decimal? CpuUsagePercent { get; set; }
    public int? MemoryUsageMb { get; set; }
    public int RestartCount { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? StoppedAt { get; set; }
    public DateTime? LastHealthCheck { get; set; }
}

public class CreateContainerHostDto
{
    public string HostId { get; set; }
    public string Hostname { get; set; }
    public string IpAddress { get; set; }
    public string HostType { get; set; } = "docker";
    public string? Region { get; set; }
    public string? AvailabilityZone { get; set; }
    public int? TotalCpuCores { get; set; }
    public int? TotalMemoryGb { get; set; }
    public int? TotalStorageGb { get; set; }
    public int MaxContainers { get; set; } = 100;
    public string Status { get; set; } = "active";
    public string? Labels { get; set; }
}

public class UpdateContainerHostDto
{
    public string? Status { get; set; }
    public decimal? UsedCpuCores { get; set; }
    public decimal? UsedMemoryGb { get; set; }
    public decimal? UsedStorageGb { get; set; }
    public int? CurrentContainers { get; set; }
    public DateTime? LastHeartbeat { get; set; }
}

public class ContainerHostDto : EntityDto<int>
{
    public string HostId { get; set; }
    public string Hostname { get; set; }
    public string IpAddress { get; set; }
    public string HostType { get; set; }
    public string? Region { get; set; }
    public string? AvailabilityZone { get; set; }
    public int? TotalCpuCores { get; set; }
    public int? TotalMemoryGb { get; set; }
    public int? TotalStorageGb { get; set; }
    public decimal? UsedCpuCores { get; set; }
    public decimal? UsedMemoryGb { get; set; }
    public decimal? UsedStorageGb { get; set; }
    public int MaxContainers { get; set; }
    public int CurrentContainers { get; set; }
    public string Status { get; set; }
    public string? Labels { get; set; }
    public DateTime? LastHeartbeat { get; set; }
}
