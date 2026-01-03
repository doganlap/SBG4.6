using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AbpSaasPlatform.Subscriptions;
using AbpSaasPlatform.Subscriptions.Dtos;

namespace AbpSaasPlatform.Application.Subscriptions;

public class SubscriptionAppService : ApplicationService, ISubscriptionAppService
{
    private readonly IRepository<Subscription, Guid> _subscriptionRepository;
    private readonly IRepository<SubscriptionPlan, int> _planRepository;

    public SubscriptionAppService(
        IRepository<Subscription, Guid> subscriptionRepository,
        IRepository<SubscriptionPlan, int> planRepository)
    {
        _subscriptionRepository = subscriptionRepository;
        _planRepository = planRepository;
    }

    public async Task<SubscriptionDto> CreateAsync(SubscriptionCreateDto input)
    {
        var plan = await _planRepository.GetAsync(input.PlanId);
        
        var subscription = new Subscription(GuidGenerator.Create())
        {
            SubscriptionId = $"SUB{GuidGenerator.Create().ToString("N")[..8].ToUpper()}",
            TenantId_Ref = input.TenantId,
            PlanId = input.PlanId,
            Status = "trial",
            StartDate = input.StartDate,
            EndDate = input.EndDate,
            BillingCycle = input.BillingCycle,
            BasePrice = plan.PriceMonthly,
            DiscountPercent = input.DiscountPercent ?? 0,
            DiscountAmount = input.DiscountAmount ?? 0,
            FinalPrice = plan.PriceMonthly - (input.DiscountAmount ?? 0) - (plan.PriceMonthly * (input.DiscountPercent ?? 0) / 100),
            Currency = plan.Currency,
            CurrentUsers = 0,
            CurrentStorageGb = 0,
            CurrentApiCalls = 0
        };

        await _subscriptionRepository.InsertAsync(subscription, autoSave: true);
        return ObjectMapper.Map<Subscription, SubscriptionDto>(subscription);
    }

    public async Task<SubscriptionDto> GetAsync(Guid id)
    {
        var subscription = await _subscriptionRepository.GetAsync(id);
        return ObjectMapper.Map<Subscription, SubscriptionDto>(subscription);
    }

    public async Task<SubscriptionDto> GetByTenantAsync(Guid tenantId)
    {
        var subscription = await _subscriptionRepository.FirstOrDefaultAsync(x => x.TenantId_Ref == tenantId);
        if (subscription == null)
        {
            throw new Exception($"No subscription found for tenant {tenantId}");
        }
        return ObjectMapper.Map<Subscription, SubscriptionDto>(subscription);
    }

    public async Task<List<SubscriptionDto>> GetListAsync()
    {
        var subscriptions = await _subscriptionRepository.GetListAsync();
        return ObjectMapper.Map<List<Subscription>, List<SubscriptionDto>>(subscriptions);
    }

    public async Task<SubscriptionDto> UpdateAsync(Guid id, SubscriptionUpdateDto input)
    {
        var subscription = await _subscriptionRepository.GetAsync(id);
        
        if (input.PlanId.HasValue)
        {
            var plan = await _planRepository.GetAsync(input.PlanId.Value);
            subscription.PlanId = input.PlanId.Value;
            subscription.BasePrice = plan.PriceMonthly;
            subscription.FinalPrice = plan.PriceMonthly - subscription.DiscountAmount - (plan.PriceMonthly * subscription.DiscountPercent / 100);
        }
        
        if (!string.IsNullOrEmpty(input.Status))
        {
            subscription.Status = input.Status;
        }
        
        if (input.EndDate.HasValue)
        {
            subscription.EndDate = input.EndDate;
        }
        
        if (!string.IsNullOrEmpty(input.CancellationReason))
        {
            subscription.CancellationReason = input.CancellationReason;
            subscription.CancelledAt = DateTime.UtcNow;
        }

        await _subscriptionRepository.UpdateAsync(subscription, autoSave: true);
        return ObjectMapper.Map<Subscription, SubscriptionDto>(subscription);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _subscriptionRepository.DeleteAsync(id);
    }

    public async Task<SubscriptionDto> CancelAsync(Guid id, string reason)
    {
        var subscription = await _subscriptionRepository.GetAsync(id);
        subscription.Status = "cancelled";
        subscription.CancelledAt = DateTime.UtcNow;
        subscription.CancellationReason = reason;
        
        await _subscriptionRepository.UpdateAsync(subscription, autoSave: true);
        return ObjectMapper.Map<Subscription, SubscriptionDto>(subscription);
    }
}
