using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AbpSaasPlatform.Tenants;
using AbpSaasPlatform.Tenants.Dtos;

namespace AbpSaasPlatform.Application.Tenants;

public class TenantSettingAppService : ApplicationService, ITenantSettingAppService
{
    private readonly IRepository<TenantSetting, int> _settingRepository;

    public TenantSettingAppService(IRepository<TenantSetting, int> settingRepository)
    {
        _settingRepository = settingRepository;
    }

    public async Task<TenantSettingDto> GetSettingAsync(string key)
    {
        var setting = await _settingRepository.FirstOrDefaultAsync(x => x.SettingKey == key);
        if (setting == null)
        {
            throw new Exception($"Setting with key '{key}' not found");
        }
        return ObjectMapper.Map<TenantSetting, TenantSettingDto>(setting);
    }

    public async Task<TenantSettingDto> SetSettingAsync(CreateOrUpdateTenantSettingDto input)
    {
        var setting = await _settingRepository.FirstOrDefaultAsync(x => x.SettingKey == input.SettingKey);
        
        if (setting == null)
        {
            setting = new TenantSetting
            {
                SettingKey = input.SettingKey,
                SettingValue = input.SettingValue,
                SettingType = input.SettingType,
                Category = input.Category,
                Description = input.Description,
                IsUserEditable = input.IsUserEditable
            };
            await _settingRepository.InsertAsync(setting, autoSave: true);
        }
        else
        {
            if (!setting.IsUserEditable && !input.IsUserEditable)
            {
                throw new Exception($"Setting '{input.SettingKey}' is not user editable");
            }
            
            setting.SettingValue = input.SettingValue;
            setting.SettingType = input.SettingType;
            setting.Category = input.Category;
            setting.Description = input.Description;
            setting.IsUserEditable = input.IsUserEditable;
            await _settingRepository.UpdateAsync(setting, autoSave: true);
        }

        return ObjectMapper.Map<TenantSetting, TenantSettingDto>(setting);
    }

    public async Task<List<TenantSettingDto>> GetSettingsByCategoryAsync(string category)
    {
        var settings = await _settingRepository.GetListAsync(x => x.Category == category);
        return ObjectMapper.Map<List<TenantSetting>, List<TenantSettingDto>>(settings.OrderBy(x => x.SettingKey).ToList());
    }

    public async Task<List<TenantSettingDto>> GetAllSettingsAsync()
    {
        var settings = await _settingRepository.GetListAsync();
        return ObjectMapper.Map<List<TenantSetting>, List<TenantSettingDto>>(settings.OrderBy(x => x.Category).ThenBy(x => x.SettingKey).ToList());
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
