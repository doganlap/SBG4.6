using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Subscriptions;

public class SubscriptionPlan : AuditedEntity<int>
{
    public string PlanCode { get; set; } = null!;
    public string PlanName { get; set; } = null!;
    public string? Description { get; set; }
    
    // Pricing
    public decimal PriceMonthly { get; set; }
    public decimal? PriceYearly { get; set; }
    public string Currency { get; set; } = "USD";
    
    // Limits
    public int MaxUsers { get; set; } = 5;
    public int MaxStorageGb { get; set; } = 10;
    public int MaxApiCallsPerMonth { get; set; } = 10000;
    
    // Features
    public string? IncludedModules { get; set; } // JSON array
    public string? Features { get; set; } // JSON key-value pairs
    
    // Status
    public bool IsActive { get; set; } = true;
    public bool IsPublic { get; set; } = true;
    public bool IsTrialAvailable { get; set; } = true;
    public int TrialDays { get; set; } = 14;
    
    // Display
    public int DisplayOrder { get; set; } = 0;
    public string? BadgeText { get; set; }
}
