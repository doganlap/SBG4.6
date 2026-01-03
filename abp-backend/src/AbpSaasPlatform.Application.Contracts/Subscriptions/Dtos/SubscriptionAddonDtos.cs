using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Subscriptions.Dtos;

public class CreateSubscriptionAddonDto
{
    public string AddonCode { get; set; }
    public string AddonName { get; set; }
    public string? Description { get; set; }
    public string AddonType { get; set; }
    public decimal PriceMonthly { get; set; }
    public decimal? PriceYearly { get; set; }
    public string Currency { get; set; } = "USD";
    public bool IsQuantityBased { get; set; } = false;
    public string? QuantityUnit { get; set; }
    public int MinQuantity { get; set; } = 1;
    public int? MaxQuantity { get; set; }
    public int? ModuleId { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateSubscriptionAddonDto
{
    public string? AddonName { get; set; }
    public string? Description { get; set; }
    public decimal? PriceMonthly { get; set; }
    public decimal? PriceYearly { get; set; }
    public bool? IsQuantityBased { get; set; }
    public string? QuantityUnit { get; set; }
    public int? MinQuantity { get; set; }
    public int? MaxQuantity { get; set; }
    public int? ModuleId { get; set; }
    public bool? IsActive { get; set; }
}

public class SubscriptionAddonDto : EntityDto<int>
{
    public string AddonCode { get; set; }
    public string AddonName { get; set; }
    public string? Description { get; set; }
    public string AddonType { get; set; }
    public decimal PriceMonthly { get; set; }
    public decimal? PriceYearly { get; set; }
    public string Currency { get; set; }
    public bool IsQuantityBased { get; set; }
    public string? QuantityUnit { get; set; }
    public int MinQuantity { get; set; }
    public int? MaxQuantity { get; set; }
    public int? ModuleId { get; set; }
    public bool IsActive { get; set; }
}
