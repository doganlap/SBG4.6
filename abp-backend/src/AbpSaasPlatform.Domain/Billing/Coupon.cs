using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Billing;

public class Coupon : AuditedEntity<int>
{
    public string CouponCode { get; set; } = null!;
    public string CouponName { get; set; } = null!;
    public string? Description { get; set; }
    
    // Discount
    public string DiscountType { get; set; } = "percentage"; // percentage, fixed_amount
    public decimal DiscountValue { get; set; }
    public string Currency { get; set; } = "USD";
    
    // Limits
    public decimal? MinimumAmount { get; set; }
    public decimal? MaximumDiscountAmount { get; set; }
    
    // Validity
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }
    
    // Usage limits
    public int? MaxRedemptions { get; set; }
    public int? MaxRedemptionsPerUser { get; set; }
    public int CurrentRedemptions { get; set; } = 0;
    
    // Applicability
    public string? ApplicablePlans { get; set; } // JSON array
    public string? ApplicableModules { get; set; } // JSON array
    
    // Status
    public bool IsActive { get; set; } = true;
    
    // Metadata
    public string? Metadata { get; set; } // JSON
}
