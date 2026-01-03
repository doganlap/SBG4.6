using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using AbpSaasPlatform.Tenants.Dtos;

namespace AbpSaasPlatform.Tenants;

public interface ITenantRoleAppService : IApplicationService
{
    Task<TenantRoleDto> CreateAsync(CreateTenantRoleDto input);
    Task<TenantRoleDto> GetAsync(int id);
    Task<TenantRoleDto> GetByCodeAsync(string roleCode);
    Task<List<TenantRoleDto>> GetListAsync(bool? isActive = null);
    Task<TenantRoleDto> UpdateAsync(int id, UpdateTenantRoleDto input);
    Task DeleteAsync(int id);
}
