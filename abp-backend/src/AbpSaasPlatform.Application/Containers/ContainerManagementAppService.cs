using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AbpSaasPlatform.Containers;
using AbpSaasPlatform.Containers.Dtos;

namespace AbpSaasPlatform.Application.Containers;

public class ContainerManagementAppService : ApplicationService, IContainerManagementAppService
{
    private readonly IRepository<ContainerImage, int> _imageRepository;
    private readonly IRepository<ContainerInstance, long> _instanceRepository;
    private readonly IRepository<ContainerHost, int> _hostRepository;

    public ContainerManagementAppService(
        IRepository<ContainerImage, int> imageRepository,
        IRepository<ContainerInstance, long> instanceRepository,
        IRepository<ContainerHost, int> hostRepository)
    {
        _imageRepository = imageRepository;
        _instanceRepository = instanceRepository;
        _hostRepository = hostRepository;
    }

    // Container Images
    public async Task<ContainerImageDto> CreateImageAsync(CreateContainerImageDto input)
    {
        var image = new ContainerImage
        {
            ImageId = input.ImageId,
            ImageName = input.ImageName,
            ImageTag = input.ImageTag,
            FullImageUrl = input.FullImageUrl,
            RegistryUrl = input.RegistryUrl,
            RegistryType = input.RegistryType,
            ImageType = input.ImageType,
            ModuleId = input.ModuleId,
            ErpnextVersion = input.ErpnextVersion,
            FrappeVersion = input.FrappeVersion,
            ImageSizeMb = input.ImageSizeMb,
            IsActive = input.IsActive,
            IsLatest = input.IsLatest,
            BuildDate = input.BuildDate,
            CommitHash = input.CommitHash
        };

        await _imageRepository.InsertAsync(image, autoSave: true);
        return ObjectMapper.Map<ContainerImage, ContainerImageDto>(image);
    }

    public async Task<ContainerImageDto> GetImageAsync(int id)
    {
        var image = await _imageRepository.GetAsync(id);
        return ObjectMapper.Map<ContainerImage, ContainerImageDto>(image);
    }

    public async Task<List<ContainerImageDto>> GetImagesByTypeAsync(string imageType)
    {
        var images = await _imageRepository.GetListAsync(x => x.ImageType == imageType && x.IsActive);
        return ObjectMapper.Map<List<ContainerImage>, List<ContainerImageDto>>(images.OrderBy(x => x.ImageName).ToList());
    }

    public async Task<List<ContainerImageDto>> GetAllImagesAsync()
    {
        var images = await _imageRepository.GetListAsync();
        return ObjectMapper.Map<List<ContainerImage>, List<ContainerImageDto>>(images.OrderBy(x => x.ImageType).ThenBy(x => x.ImageName).ToList());
    }

    public async Task<ContainerImageDto> UpdateImageAsync(int id, UpdateContainerImageDto input)
    {
        var image = await _imageRepository.GetAsync(id);
        
        if (!string.IsNullOrEmpty(input.ImageTag))
            image.ImageTag = input.ImageTag;
        if (!string.IsNullOrEmpty(input.FullImageUrl))
            image.FullImageUrl = input.FullImageUrl;
        if (input.IsActive.HasValue)
            image.IsActive = input.IsActive.Value;
        if (input.IsLatest.HasValue)
            image.IsLatest = input.IsLatest.Value;
        if (input.BuildDate.HasValue)
            image.BuildDate = input.BuildDate.Value;
        if (!string.IsNullOrEmpty(input.CommitHash))
            image.CommitHash = input.CommitHash;

        await _imageRepository.UpdateAsync(image, autoSave: true);
        return ObjectMapper.Map<ContainerImage, ContainerImageDto>(image);
    }

    public async Task DeleteImageAsync(int id)
    {
        await _imageRepository.DeleteAsync(id);
    }

    // Container Instances
    public async Task<ContainerInstanceDto> CreateInstanceAsync(CreateContainerInstanceDto input)
    {
        var instance = new ContainerInstance
        {
            InstanceId = $"INST-{GuidGenerator.Create().ToString("N")[..12].ToUpper()}",
            TenantId = input.TenantId,
            ImageId = input.ImageId,
            ContainerName = input.ContainerName,
            InstanceType = input.InstanceType,
            HostId = input.HostId,
            InternalPort = input.InternalPort,
            ExternalPort = input.ExternalPort,
            CpuLimit = input.CpuLimit,
            MemoryLimitMb = input.MemoryLimitMb,
            Status = "creating",
            HealthStatus = "unknown"
        };

        await _instanceRepository.InsertAsync(instance, autoSave: true);
        return ObjectMapper.Map<ContainerInstance, ContainerInstanceDto>(instance);
    }

    public async Task<ContainerInstanceDto> GetInstanceAsync(long id)
    {
        var instance = await _instanceRepository.GetAsync(id);
        return ObjectMapper.Map<ContainerInstance, ContainerInstanceDto>(instance);
    }

    public async Task<List<ContainerInstanceDto>> GetInstancesByTenantAsync(Guid tenantId)
    {
        var instances = await _instanceRepository.GetListAsync(x => x.TenantId == tenantId);
        return ObjectMapper.Map<List<ContainerInstance>, List<ContainerInstanceDto>>(instances.OrderByDescending(x => x.CreationTime).ToList());
    }

    public async Task<List<ContainerInstanceDto>> GetInstancesByStatusAsync(string status)
    {
        var instances = await _instanceRepository.GetListAsync(x => x.Status == status);
        return ObjectMapper.Map<List<ContainerInstance>, List<ContainerInstanceDto>>(instances.OrderByDescending(x => x.CreationTime).ToList());
    }

    public async Task<ContainerInstanceDto> UpdateInstanceStatusAsync(long id, string status, string healthStatus)
    {
        var instance = await _instanceRepository.GetAsync(id);
        instance.Status = status;
        instance.HealthStatus = healthStatus;
        instance.LastHealthCheck = DateTime.UtcNow;
        
        if (status == "running" && instance.StartedAt == null)
            instance.StartedAt = DateTime.UtcNow;
        if (status == "stopped" && instance.StoppedAt == null)
            instance.StoppedAt = DateTime.UtcNow;

        await _instanceRepository.UpdateAsync(instance, autoSave: true);
        return ObjectMapper.Map<ContainerInstance, ContainerInstanceDto>(instance);
    }

    public async Task DeleteInstanceAsync(long id)
    {
        await _instanceRepository.DeleteAsync(id);
    }

    // Container Hosts
    public async Task<ContainerHostDto> CreateHostAsync(CreateContainerHostDto input)
    {
        var host = new ContainerHost
        {
            HostId = input.HostId,
            Hostname = input.Hostname,
            IpAddress = input.IpAddress,
            HostType = input.HostType,
            Region = input.Region,
            AvailabilityZone = input.AvailabilityZone,
            TotalCpuCores = input.TotalCpuCores,
            TotalMemoryGb = input.TotalMemoryGb,
            TotalStorageGb = input.TotalStorageGb,
            MaxContainers = input.MaxContainers,
            Status = input.Status,
            Labels = input.Labels,
            LastHeartbeat = DateTime.UtcNow
        };

        await _hostRepository.InsertAsync(host, autoSave: true);
        return ObjectMapper.Map<ContainerHost, ContainerHostDto>(host);
    }

    public async Task<ContainerHostDto> GetHostAsync(int id)
    {
        var host = await _hostRepository.GetAsync(id);
        return ObjectMapper.Map<ContainerHost, ContainerHostDto>(host);
    }

    public async Task<List<ContainerHostDto>> GetAllHostsAsync()
    {
        var hosts = await _hostRepository.GetListAsync();
        return ObjectMapper.Map<List<ContainerHost>, List<ContainerHostDto>>(hosts.OrderBy(x => x.Hostname).ToList());
    }

    public async Task<ContainerHostDto> UpdateHostAsync(int id, UpdateContainerHostDto input)
    {
        var host = await _hostRepository.GetAsync(id);
        
        if (!string.IsNullOrEmpty(input.Status))
            host.Status = input.Status;
        if (input.UsedCpuCores.HasValue)
            host.UsedCpuCores = input.UsedCpuCores.Value;
        if (input.UsedMemoryGb.HasValue)
            host.UsedMemoryGb = input.UsedMemoryGb.Value;
        if (input.UsedStorageGb.HasValue)
            host.UsedStorageGb = input.UsedStorageGb.Value;
        if (input.CurrentContainers.HasValue)
            host.CurrentContainers = input.CurrentContainers.Value;
        if (input.LastHeartbeat.HasValue)
            host.LastHeartbeat = input.LastHeartbeat.Value;

        await _hostRepository.UpdateAsync(host, autoSave: true);
        return ObjectMapper.Map<ContainerHost, ContainerHostDto>(host);
    }

    public async Task DeleteHostAsync(int id)
    {
        await _hostRepository.DeleteAsync(id);
    }
}
