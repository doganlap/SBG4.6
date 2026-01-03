using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace AbpSaasPlatform.Modules;

public class TenantModule : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid TenantId_Ref { get; set; }
    public string ModuleKey { get; set; } = null!;
    public string ModuleName { get; set; } = null!;
    public bool IsEnabled { get; set; } = true;
    public DateTime? EnabledAt { get; set; }
}
