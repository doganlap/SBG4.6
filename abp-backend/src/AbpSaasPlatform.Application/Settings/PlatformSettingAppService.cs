using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AbpSaasPlatform.Settings;
using AbpSaasPlatform.Settings.Dtos;

namespace AbpSaasPlatform.Application.Settings;

public class PlatformSettingAppService : ApplicationService, IPlatformSettingAppService
{
    private readonly IRepository<PlatformSetting, int> _settingRepository;

    public PlatformSettingAppService(IRepository<PlatformSetting, int> settingRepository)
    {
        _settingRepository = settingRepository;
    }

    public async Task<PlatformSettingDto> GetSettingAsync(string key)
    {
        var setting = await _settingRepository.FirstOrDefaultAsync(x => x.SettingKey == key);
        if (setting == null)
        {
            throw new Exception($"Setting with key '{key}' not found");
        }
        return ObjectMapper.Map<PlatformSetting, PlatformSettingDto>(setting);
    }

    public async Task<PlatformSettingDto> SetSettingAsync(CreateOrUpdateSettingDto input)
    {
        var setting = await _settingRepository.FirstOrDefaultAsync(x => x.SettingKey == input.SettingKey);
        
        if (setting == null)
        {
            setting = new PlatformSetting
            {
                SettingKey = input.SettingKey,
                SettingValue = input.SettingValue,
                SettingType = input.SettingType,
                Category = input.Category,
                Description = input.Description,
                IsSensitive = input.IsSensitive
            };
            await _settingRepository.InsertAsync(setting, autoSave: true);
        }
        else
        {
            setting.SettingValue = input.SettingValue;
            setting.SettingType = input.SettingType;
            setting.Category = input.Category;
            setting.Description = input.Description;
            setting.IsSensitive = input.IsSensitive;
            await _settingRepository.UpdateAsync(setting, autoSave: true);
        }

        return ObjectMapper.Map<PlatformSetting, PlatformSettingDto>(setting);
    }

    public async Task<List<PlatformSettingDto>> GetSettingsByCategoryAsync(string category)
    {
        var settings = await _settingRepository.GetListAsync(x => x.Category == category);
        return ObjectMapper.Map<List<PlatformSetting>, List<PlatformSettingDto>>(settings.OrderBy(x => x.SettingKey).ToList());
    }

    public async Task<List<PlatformSettingDto>> GetAllSettingsAsync()
    {
        var settings = await _settingRepository.GetListAsync();
        return ObjectMapper.Map<List<PlatformSetting>, List<PlatformSettingDto>>(settings.OrderBy(x => x.Category).ThenBy(x => x.SettingKey).ToList());
    }

    public async Task DeleteSettingAsync(string key)
    {
        var setting = await _settingRepository.FirstOrDefaultAsync(x => x.SettingKey == key);
        if (setting != null)
        {
            await _settingRepository.DeleteAsync(setting);
        }
    }
}
