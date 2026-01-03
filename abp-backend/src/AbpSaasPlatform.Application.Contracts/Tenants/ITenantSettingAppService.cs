using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using AbpSaasPlatform.Tenants.Dtos;

namespace AbpSaasPlatform.Tenants;

public interface ITenantSettingAppService : IApplicationService
{
    Task<TenantSettingDto> GetSettingAsync(string key);
    Task<TenantSettingDto> SetSettingAsync(CreateOrUpdateTenantSettingDto input);
    Task<List<TenantSettingDto>> GetSettingsByCategoryAsync(string category);
    Task<List<TenantSettingDto>> GetAllSettingsAsync();
    Task DeleteSettingAsync(string key);
}
