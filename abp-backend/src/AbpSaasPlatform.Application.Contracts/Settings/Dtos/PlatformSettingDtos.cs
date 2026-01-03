using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Settings.Dtos;

public class CreateOrUpdateSettingDto
{
    public string SettingKey { get; set; }
    public string? SettingValue { get; set; }
    public string SettingType { get; set; } = "string";
    public string? Category { get; set; }
    public string? Description { get; set; }
    public bool IsSensitive { get; set; } = false;
}

public class PlatformSettingDto : EntityDto<int>
{
    public string SettingKey { get; set; }
    public string? SettingValue { get; set; }
    public string SettingType { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public bool IsSensitive { get; set; }
}
