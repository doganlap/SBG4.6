using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using AbpSaasPlatform.Containers.Dtos;

namespace AbpSaasPlatform.Containers;

public interface IContainerManagementAppService : IApplicationService
{
    // Container Images
    Task<ContainerImageDto> CreateImageAsync(CreateContainerImageDto input);
    Task<ContainerImageDto> GetImageAsync(int id);
    Task<List<ContainerImageDto>> GetImagesByTypeAsync(string imageType);
    Task<List<ContainerImageDto>> GetAllImagesAsync();
    Task<ContainerImageDto> UpdateImageAsync(int id, UpdateContainerImageDto input);
    Task DeleteImageAsync(int id);
    
    // Container Instances
    Task<ContainerInstanceDto> CreateInstanceAsync(CreateContainerInstanceDto input);
    Task<ContainerInstanceDto> GetInstanceAsync(long id);
    Task<List<ContainerInstanceDto>> GetInstancesByTenantAsync(Guid tenantId);
    Task<List<ContainerInstanceDto>> GetInstancesByStatusAsync(string status);
    Task<ContainerInstanceDto> UpdateInstanceStatusAsync(long id, string status, string healthStatus);
    Task DeleteInstanceAsync(long id);
    
    // Container Hosts
    Task<ContainerHostDto> CreateHostAsync(CreateContainerHostDto input);
    Task<ContainerHostDto> GetHostAsync(int id);
    Task<List<ContainerHostDto>> GetAllHostsAsync();
    Task<ContainerHostDto> UpdateHostAsync(int id, UpdateContainerHostDto input);
    Task DeleteHostAsync(int id);
}
