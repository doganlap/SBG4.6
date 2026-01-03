using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.ModuleEntitlements.Dtos;

public class TenantModuleCreateDto
{
    public string ModuleKey { get; set; }
    public string ModuleName { get; set; }
}

public class TenantModuleDto : EntityDto<Guid>
{
    public Guid TenantId { get; set; }
    public string ModuleKey { get; set; }
    public string ModuleName { get; set; }
    public bool IsEnabled { get; set; }
    public DateTime? EnabledAt { get; set; }
}
