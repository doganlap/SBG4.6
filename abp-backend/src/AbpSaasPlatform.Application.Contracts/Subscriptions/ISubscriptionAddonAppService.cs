using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using AbpSaasPlatform.Subscriptions.Dtos;

namespace AbpSaasPlatform.Subscriptions;

public interface ISubscriptionAddonAppService : IApplicationService
{
    Task<SubscriptionAddonDto> CreateAsync(CreateSubscriptionAddonDto input);
    Task<SubscriptionAddonDto> GetAsync(int id);
    Task<List<SubscriptionAddonDto>> GetListAsync(string? addonType = null, bool? isActive = null);
    Task<SubscriptionAddonDto> UpdateAsync(int id, UpdateSubscriptionAddonDto input);
    Task DeleteAsync(int id);
    Task<List<SubscriptionAddonDto>> GetActiveAddonsAsync();
}
