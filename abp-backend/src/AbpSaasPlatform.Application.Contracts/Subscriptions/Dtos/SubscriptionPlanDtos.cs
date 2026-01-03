using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Subscriptions.Dtos;

public class SubscriptionPlanCreateDto
{
    public string PlanCode { get; set; }
    public string PlanName { get; set; }
    public string? Description { get; set; }
    public decimal PriceMonthly { get; set; }
    public decimal? PriceYearly { get; set; }
    public string Currency { get; set; } = "USD";
    public int MaxUsers { get; set; } = 5;
    public int MaxStorageGb { get; set; } = 10;
    public int MaxApiCallsPerMonth { get; set; } = 10000;
    public string? IncludedModules { get; set; }
    public string? Features { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsPublic { get; set; } = true;
    public bool IsTrialAvailable { get; set; } = true;
    public int TrialDays { get; set; } = 14;
    public int DisplayOrder { get; set; } = 0;
    public string? BadgeText { get; set; }
}

public class SubscriptionPlanUpdateDto
{
    public string? PlanName { get; set; }
    public string? Description { get; set; }
    public decimal? PriceMonthly { get; set; }
    public decimal? PriceYearly { get; set; }
    public int? MaxUsers { get; set; }
    public int? MaxStorageGb { get; set; }
    public int? MaxApiCallsPerMonth { get; set; }
    public string? IncludedModules { get; set; }
    public string? Features { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsPublic { get; set; }
    public bool? IsTrialAvailable { get; set; }
    public int? TrialDays { get; set; }
    public int? DisplayOrder { get; set; }
    public string? BadgeText { get; set; }
}

public class SubscriptionPlanDto : EntityDto<int>
{
    public string PlanCode { get; set; }
    public string PlanName { get; set; }
    public string? Description { get; set; }
    public decimal PriceMonthly { get; set; }
    public decimal? PriceYearly { get; set; }
    public string Currency { get; set; }
    public int MaxUsers { get; set; }
    public int MaxStorageGb { get; set; }
    public int MaxApiCallsPerMonth { get; set; }
    public string? IncludedModules { get; set; }
    public string? Features { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublic { get; set; }
    public bool IsTrialAvailable { get; set; }
    public int TrialDays { get; set; }
    public int DisplayOrder { get; set; }
    public string? BadgeText { get; set; }
}
