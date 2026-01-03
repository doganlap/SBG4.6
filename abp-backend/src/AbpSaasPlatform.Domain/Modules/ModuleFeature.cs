using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Modules;

public class ModuleFeature : AuditedEntity<int>
{
    public int ModuleId { get; set; }
    public Module Module { get; set; } = null!;
    
    public string FeatureCode { get; set; } = null!;
    public string FeatureName { get; set; } = null!;
    public string? Description { get; set; }
    
    public bool IsEnabled { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    
    public string? Configuration { get; set; } // JSON
}
