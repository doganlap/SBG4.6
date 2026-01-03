using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Modules;

public class Module : AuditedEntity<int>
{
    public string ModuleCode { get; set; } = null!;
    public string ModuleName { get; set; } = null!;
    public string? Description { get; set; }
    public string Category { get; set; } = "core"; // core, domain, standalone, addon
    
    // Technical
    public string? FrappeAppName { get; set; }
    public string? GithubUrl { get; set; }
    public string? Version { get; set; }
    
    // Features
    public string? Features { get; set; } // JSON
    public string? Doctypes { get; set; } // JSON array
    public string? Dependencies { get; set; } // JSON array
    
    // Pricing
    public bool IsFree { get; set; } = false;
    public decimal AddonPriceMonthly { get; set; } = 0;
    public decimal AddonPriceYearly { get; set; } = 0;
    
    // Status
    public bool IsActive { get; set; } = true;
    public bool IsBeta { get; set; } = false;
    
    // Display
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public int DisplayOrder { get; set; } = 0;
}
