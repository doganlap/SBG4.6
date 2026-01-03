using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using AbpSaasPlatform.Tenants.Dtos;

namespace AbpSaasPlatform.Tenants;

public interface ITenantUserAppService : IApplicationService
{
    Task<TenantUserDto> CreateAsync(CreateTenantUserDto input);
    Task<TenantUserDto> GetAsync(Guid id);
    Task<TenantUserDto> GetByEmailAsync(string email);
    Task<List<TenantUserDto>> GetListAsync();
    Task<List<TenantUserDto>> GetByDepartmentAsync(string department);
    Task<TenantUserDto> UpdateAsync(Guid id, UpdateTenantUserDto input);
    Task DeleteAsync(Guid id);
    Task<TenantUserDto> ActivateAsync(Guid id);
    Task<TenantUserDto> DeactivateAsync(Guid id);
    Task<TenantUserDto> UpdateLastLoginAsync(Guid id, string ipAddress);
}
