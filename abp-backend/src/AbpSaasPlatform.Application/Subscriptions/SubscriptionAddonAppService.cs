using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AbpSaasPlatform.Subscriptions;
using AbpSaasPlatform.Subscriptions.Dtos;

namespace AbpSaasPlatform.Application.Subscriptions;

public class SubscriptionAddonAppService : ApplicationService, ISubscriptionAddonAppService
{
    private readonly IRepository<SubscriptionAddon, int> _addonRepository;

    public SubscriptionAddonAppService(IRepository<SubscriptionAddon, int> addonRepository)
    {
        _addonRepository = addonRepository;
    }

    public async Task<SubscriptionAddonDto> CreateAsync(CreateSubscriptionAddonDto input)
    {
        var addon = new SubscriptionAddon
        {
            AddonCode = input.AddonCode,
            AddonName = input.AddonName,
            Description = input.Description,
            AddonType = input.AddonType,
            PriceMonthly = input.PriceMonthly,
            PriceYearly = input.PriceYearly,
            Currency = input.Currency,
            IsQuantityBased = input.IsQuantityBased,
            QuantityUnit = input.QuantityUnit,
            MinQuantity = input.MinQuantity,
            MaxQuantity = input.MaxQuantity,
            ModuleId = input.ModuleId,
            IsActive = input.IsActive
        };

        await _addonRepository.InsertAsync(addon, autoSave: true);
        return ObjectMapper.Map<SubscriptionAddon, SubscriptionAddonDto>(addon);
    }

    public async Task<SubscriptionAddonDto> GetAsync(int id)
    {
        var addon = await _addonRepository.GetAsync(id);
        return ObjectMapper.Map<SubscriptionAddon, SubscriptionAddonDto>(addon);
    }

    public async Task<List<SubscriptionAddonDto>> GetListAsync(string? addonType = null, bool? isActive = null)
    {
        var query = await _addonRepository.GetQueryableAsync();
        var addons = query.AsQueryable();
        
        if (addonType != null)
            addons = addons.Where(x => x.AddonType == addonType);
        if (isActive.HasValue)
            addons = addons.Where(x => x.IsActive == isActive.Value);

        var list = addons.OrderBy(x => x.AddonName).ToList();
        return ObjectMapper.Map<List<SubscriptionAddon>, List<SubscriptionAddonDto>>(list);
    }

    public async Task<SubscriptionAddonDto> UpdateAsync(int id, UpdateSubscriptionAddonDto input)
    {
        var addon = await _addonRepository.GetAsync(id);
        
        if (!string.IsNullOrEmpty(input.AddonName))
            addon.AddonName = input.AddonName;
        if (!string.IsNullOrEmpty(input.Description))
            addon.Description = input.Description;
        if (input.PriceMonthly.HasValue)
            addon.PriceMonthly = input.PriceMonthly.Value;
        if (input.PriceYearly.HasValue)
            addon.PriceYearly = input.PriceYearly.Value;
        if (input.IsQuantityBased.HasValue)
            addon.IsQuantityBased = input.IsQuantityBased.Value;
        if (input.QuantityUnit != null)
            addon.QuantityUnit = input.QuantityUnit;
        if (input.MinQuantity.HasValue)
            addon.MinQuantity = input.MinQuantity.Value;
        if (input.MaxQuantity.HasValue)
            addon.MaxQuantity = input.MaxQuantity.Value;
        if (input.ModuleId.HasValue)
            addon.ModuleId = input.ModuleId.Value;
        if (input.IsActive.HasValue)
            addon.IsActive = input.IsActive.Value;

        await _addonRepository.UpdateAsync(addon, autoSave: true);
        return ObjectMapper.Map<SubscriptionAddon, SubscriptionAddonDto>(addon);
    }

    public async Task DeleteAsync(int id)
    {
        await _addonRepository.DeleteAsync(id);
    }

    public async Task<List<SubscriptionAddonDto>> GetActiveAddonsAsync()
    {
        var addons = await _addonRepository.GetListAsync(x => x.IsActive);
        return ObjectMapper.Map<List<SubscriptionAddon>, List<SubscriptionAddonDto>>(addons.OrderBy(x => x.AddonName).ToList());
    }
}
