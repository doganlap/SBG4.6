using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Billing;

public class TaxRate : AuditedEntity<int>
{
    public string TaxCode { get; set; } = null!;
    public string TaxName { get; set; } = null!;
    public string? Description { get; set; }
    
    public decimal Rate { get; set; } // Percentage (e.g., 8.5 for 8.5%)
    
    // Applicability
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? City { get; set; }
    
    // Tax type
    public string TaxType { get; set; } = "sales"; // sales, vat, gst, other
    
    // Status
    public bool IsActive { get; set; } = true;
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveUntil { get; set; }
    
    // Metadata
    public string? Metadata { get; set; } // JSON
}
