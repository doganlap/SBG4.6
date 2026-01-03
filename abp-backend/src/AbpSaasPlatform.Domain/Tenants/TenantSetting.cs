using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Tenants;

public class TenantSetting : AuditedEntity<int>
{
    public string SettingKey { get; set; } = null!;
    public string? SettingValue { get; set; }
    public string SettingType { get; set; } = "string"; // string, number, boolean, json
    public string? Category { get; set; }
    public string? Description { get; set; }
    public bool IsUserEditable { get; set; } = true;
}
