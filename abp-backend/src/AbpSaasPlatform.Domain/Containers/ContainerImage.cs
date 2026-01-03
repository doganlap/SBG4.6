using Volo.Abp.Domain.Entities.Auditing;
using AbpSaasPlatform.Modules;

namespace AbpSaasPlatform.Containers;

public class ContainerImage : AuditedEntity<int>
{
    public string ImageId { get; set; } = null!;
    
    // Image info
    public string ImageName { get; set; } = null!;
    public string ImageTag { get; set; } = null!;
    public string FullImageUrl { get; set; } = null!;
    
    // Registry
    public string? RegistryUrl { get; set; }
    public string RegistryType { get; set; } = "dockerhub"; // dockerhub, ecr, gcr, acr, private
    
    // Type
    public string ImageType { get; set; } = null!; // base, module, custom, tenant
    
    // Module (if module image)
    public int? ModuleId { get; set; }
    public Module? Module { get; set; }
    
    // Version info
    public string? ErpnextVersion { get; set; }
    public string? FrappeVersion { get; set; }
    
    // Size
    public int? ImageSizeMb { get; set; }
    
    // Status
    public bool IsActive { get; set; } = true;
    public bool IsLatest { get; set; } = false;
    
    // Build info
    public DateTime? BuildDate { get; set; }
    public string? CommitHash { get; set; }
}
