using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AbpSaasPlatform.Modules;
using AbpSaasPlatform.ModuleEntitlements;
using AbpSaasPlatform.ModuleEntitlements.Dtos;

namespace AbpSaasPlatform.Application.ModuleEntitlements;

public class ModuleEntitlementAppService : ApplicationService, IModuleEntitlementAppService
{
    private readonly IRepository<TenantModule, Guid> _tenantModuleRepository;

    public ModuleEntitlementAppService(IRepository<TenantModule, Guid> tenantModuleRepository)
    {
        _tenantModuleRepository = tenantModuleRepository;
    }

    public async Task<TenantModuleDto> AssignAsync(Guid tenantId, TenantModuleCreateDto input)
    {
        var tenantModule = new TenantModule(GuidGenerator.Create())
        {
            TenantId = tenantId,
            TenantId_Ref = tenantId,
            ModuleKey = input.ModuleKey,
            ModuleName = input.ModuleName,
            IsEnabled = true,
            EnabledAt = DateTime.UtcNow
        };

        await _tenantModuleRepository.InsertAsync(tenantModule, autoSave: true);
        return ObjectMapper.Map<TenantModule, TenantModuleDto>(tenantModule);
    }

    public async Task<List<TenantModuleDto>> GetListByTenantAsync(Guid tenantId)
    {
        var modules = await _tenantModuleRepository.GetListAsync(x => x.TenantId_Ref == tenantId);
        return ObjectMapper.Map<List<TenantModule>, List<TenantModuleDto>>(modules);
    }

    public async Task RemoveAsync(Guid tenantId, string moduleKey)
    {
        var module = await _tenantModuleRepository.FirstOrDefaultAsync(
            x => x.TenantId_Ref == tenantId && x.ModuleKey == moduleKey);
        
        if (module != null)
        {
            await _tenantModuleRepository.DeleteAsync(module);
        }
    }
}
