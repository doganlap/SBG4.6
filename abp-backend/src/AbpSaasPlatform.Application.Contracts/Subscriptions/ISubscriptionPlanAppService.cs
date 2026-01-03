using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using AbpSaasPlatform.Subscriptions.Dtos;

namespace AbpSaasPlatform.Subscriptions;

public interface ISubscriptionPlanAppService : IApplicationService
{
    Task<SubscriptionPlanDto> CreateAsync(SubscriptionPlanCreateDto input);
    Task<SubscriptionPlanDto> GetAsync(int id);
    Task<List<SubscriptionPlanDto>> GetListAsync();
    Task<SubscriptionPlanDto> UpdateAsync(int id, SubscriptionPlanUpdateDto input);
    Task DeleteAsync(int id);
    Task<List<SubscriptionPlanDto>> GetPublicPlansAsync();
}
