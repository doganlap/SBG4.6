using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AbpSaasPlatform.Subscriptions;
using AbpSaasPlatform.Subscriptions.Dtos;

namespace AbpSaasPlatform.Application.Subscriptions;

public class SubscriptionPlanAppService : ApplicationService, ISubscriptionPlanAppService
{
    private readonly IRepository<SubscriptionPlan, int> _planRepository;

    public SubscriptionPlanAppService(IRepository<SubscriptionPlan, int> planRepository)
    {
        _planRepository = planRepository;
    }

    public async Task<SubscriptionPlanDto> CreateAsync(SubscriptionPlanCreateDto input)
    {
        var plan = new SubscriptionPlan
        {
            PlanCode = input.PlanCode,
            PlanName = input.PlanName,
            Description = input.Description,
            PriceMonthly = input.PriceMonthly,
            PriceYearly = input.PriceYearly,
            Currency = input.Currency,
            MaxUsers = input.MaxUsers,
            MaxStorageGb = input.MaxStorageGb,
            MaxApiCallsPerMonth = input.MaxApiCallsPerMonth,
            IncludedModules = input.IncludedModules,
            Features = input.Features,
            IsActive = input.IsActive,
            IsPublic = input.IsPublic,
            IsTrialAvailable = input.IsTrialAvailable,
            TrialDays = input.TrialDays,
            DisplayOrder = input.DisplayOrder,
            BadgeText = input.BadgeText
        };

        await _planRepository.InsertAsync(plan, autoSave: true);
        return ObjectMapper.Map<SubscriptionPlan, SubscriptionPlanDto>(plan);
    }

    public async Task<SubscriptionPlanDto> GetAsync(int id)
    {
        var plan = await _planRepository.GetAsync(id);
        return ObjectMapper.Map<SubscriptionPlan, SubscriptionPlanDto>(plan);
    }

    public async Task<List<SubscriptionPlanDto>> GetListAsync()
    {
        var plans = await _planRepository.GetListAsync();
        return ObjectMapper.Map<List<SubscriptionPlan>, List<SubscriptionPlanDto>>(plans);
    }

    public async Task<SubscriptionPlanDto> UpdateAsync(int id, SubscriptionPlanUpdateDto input)
    {
        var plan = await _planRepository.GetAsync(id);
        
        if (!string.IsNullOrEmpty(input.PlanName))
            plan.PlanName = input.PlanName;
        if (!string.IsNullOrEmpty(input.Description))
            plan.Description = input.Description;
        if (input.PriceMonthly.HasValue)
            plan.PriceMonthly = input.PriceMonthly.Value;
        if (input.PriceYearly.HasValue)
            plan.PriceYearly = input.PriceYearly.Value;
        if (input.MaxUsers.HasValue)
            plan.MaxUsers = input.MaxUsers.Value;
        if (input.MaxStorageGb.HasValue)
            plan.MaxStorageGb = input.MaxStorageGb.Value;
        if (input.MaxApiCallsPerMonth.HasValue)
            plan.MaxApiCallsPerMonth = input.MaxApiCallsPerMonth.Value;
        if (!string.IsNullOrEmpty(input.IncludedModules))
            plan.IncludedModules = input.IncludedModules;
        if (!string.IsNullOrEmpty(input.Features))
            plan.Features = input.Features;
        if (input.IsActive.HasValue)
            plan.IsActive = input.IsActive.Value;
        if (input.IsPublic.HasValue)
            plan.IsPublic = input.IsPublic.Value;
        if (input.IsTrialAvailable.HasValue)
            plan.IsTrialAvailable = input.IsTrialAvailable.Value;
        if (input.TrialDays.HasValue)
            plan.TrialDays = input.TrialDays.Value;
        if (input.DisplayOrder.HasValue)
            plan.DisplayOrder = input.DisplayOrder.Value;
        if (input.BadgeText != null)
            plan.BadgeText = input.BadgeText;

        await _planRepository.UpdateAsync(plan, autoSave: true);
        return ObjectMapper.Map<SubscriptionPlan, SubscriptionPlanDto>(plan);
    }

    public async Task DeleteAsync(int id)
    {
        await _planRepository.DeleteAsync(id);
    }

    public async Task<List<SubscriptionPlanDto>> GetPublicPlansAsync()
    {
        var plans = await _planRepository.GetListAsync(x => x.IsPublic && x.IsActive);
        return ObjectMapper.Map<List<SubscriptionPlan>, List<SubscriptionPlanDto>>(plans.OrderBy(x => x.DisplayOrder).ToList());
    }
}
