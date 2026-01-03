using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Modules;

namespace AbpSaasPlatform.Subscriptions;

public class SubscriptionAddon : AuditedEntity<int>
{
    public string AddonCode { get; set; } = null!;
    public string AddonName { get; set; } = null!;
    public string? Description { get; set; }
    
    // Type
    public string AddonType { get; set; } = null!; // module, users, storage, api_calls, feature, support
    
    // Pricing
    public decimal PriceMonthly { get; set; }
    public decimal? PriceYearly { get; set; }
    public string Currency { get; set; } = "USD";
    
    // Quantity
    public bool IsQuantityBased { get; set; } = false;
    public string? QuantityUnit { get; set; }
    public int MinQuantity { get; set; } = 1;
    public int? MaxQuantity { get; set; }
    
    // Module specific
    public int? ModuleId { get; set; }
    public Module? Module { get; set; }
    
    // Status
    public bool IsActive { get; set; } = true;
}
