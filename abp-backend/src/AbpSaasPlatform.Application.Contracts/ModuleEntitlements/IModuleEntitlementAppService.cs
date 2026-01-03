using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using AbpSaasPlatform.ModuleEntitlements.Dtos;

namespace AbpSaasPlatform.ModuleEntitlements;

public interface IModuleEntitlementAppService : IApplicationService
{
    Task<TenantModuleDto> AssignAsync(Guid tenantId, TenantModuleCreateDto input);
    Task<List<TenantModuleDto>> GetListByTenantAsync(Guid tenantId);
    Task RemoveAsync(Guid tenantId, string moduleKey);
}
