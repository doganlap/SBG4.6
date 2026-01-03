using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Tenants.Dtos;

public class CreateOrUpdateTenantSettingDto
{
    public string SettingKey { get; set; }
    public string? SettingValue { get; set; }
    public string SettingType { get; set; } = "string";
    public string? Category { get; set; }
    public string? Description { get; set; }
    public bool IsUserEditable { get; set; } = true;
}

public class TenantSettingDto : EntityDto<int>
{
    public string SettingKey { get; set; }
    public string? SettingValue { get; set; }
    public string SettingType { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public bool IsUserEditable { get; set; }
}
