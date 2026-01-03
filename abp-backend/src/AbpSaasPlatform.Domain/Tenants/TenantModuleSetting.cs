using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Tenants;

public class TenantModuleSetting : AuditedEntity<int>
{
    public string ModuleCode { get; set; } = null!;
    
    // Configuration
    public string? Settings { get; set; } // JSON
    public string? CustomFields { get; set; } // JSON
    public string? WorkflowConfig { get; set; } // JSON
    
    // Features
    public string? EnabledFeatures { get; set; } // JSON array
    public string? DisabledFeatures { get; set; } // JSON array
    
    // UI Customization
    public string? DashboardConfig { get; set; } // JSON
    public string? ListViewConfig { get; set; } // JSON
    public string? FormLayoutConfig { get; set; } // JSON
}
