using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Tenants.Dtos;

public class TenantCreateDto
{
    public string Name { get; set; }
    public string Domain { get; set; }
    public string Plan { get; set; }
}

public class TenantUpdateDto
{
    public string Name { get; set; }
    public string Plan { get; set; }
    public int MaxUsers { get; set; }
    public long MaxStorage { get; set; }
}

public class TenantDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public string Domain { get; set; }
    public string Plan { get; set; }
    public string Status { get; set; }
    public string ErpnextSiteName { get; set; }
    public DateTime? ActivatedAt { get; set; }
    public int MaxUsers { get; set; }
    public long MaxStorage { get; set; }
}
