using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using AbpSaasPlatform.Settings.Dtos;

namespace AbpSaasPlatform.Settings;

public interface IPlatformSettingAppService : IApplicationService
{
    Task<PlatformSettingDto> GetSettingAsync(string key);
    Task<PlatformSettingDto> SetSettingAsync(CreateOrUpdateSettingDto input);
    Task<List<PlatformSettingDto>> GetSettingsByCategoryAsync(string category);
    Task<List<PlatformSettingDto>> GetAllSettingsAsync();
    Task DeleteSettingAsync(string key);
}
