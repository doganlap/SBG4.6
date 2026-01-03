using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Settings;

public class PlatformSetting : AuditedEntity<int>
{
    public string SettingKey { get; set; } = null!;
    public string? SettingValue { get; set; }
    public string SettingType { get; set; } = "string"; // string, number, boolean, json
    public string? Category { get; set; }
    public string? Description { get; set; }
    public bool IsSensitive { get; set; }
}
