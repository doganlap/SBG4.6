using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using AbpSaasPlatform.Subscriptions.Dtos;

namespace AbpSaasPlatform.Subscriptions;

public interface ISubscriptionAppService : IApplicationService
{
    Task<SubscriptionDto> CreateAsync(SubscriptionCreateDto input);
    Task<SubscriptionDto> GetAsync(Guid id);
    Task<SubscriptionDto> GetByTenantAsync(Guid tenantId);
    Task<List<SubscriptionDto>> GetListAsync();
    Task<SubscriptionDto> UpdateAsync(Guid id, SubscriptionUpdateDto input);
    Task DeleteAsync(Guid id);
    Task<SubscriptionDto> CancelAsync(Guid id, string reason);
}
