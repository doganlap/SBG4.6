using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using AbpSaasPlatform.Tenants.Dtos;

namespace AbpSaasPlatform.Tenants;

public interface ITenantAppService : IApplicationService
{
    Task<TenantDto> CreateAsync(TenantCreateDto input);
    Task<TenantDto> GetAsync(Guid id);
    Task<List<TenantDto>> GetListAsync();
    Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input);
    Task DeleteAsync(Guid id);
}
