using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Modules.Dtos;

public class CreateModuleDto
{
    public string ModuleCode { get; set; }
    public string ModuleName { get; set; }
    public string? Description { get; set; }
    public string Category { get; set; } = "core";
    public string? FrappeAppName { get; set; }
    public string? GithubUrl { get; set; }
    public string? Version { get; set; }
    public string? Features { get; set; }
    public string? Doctypes { get; set; }
    public string? Dependencies { get; set; }
    public bool IsFree { get; set; } = false;
    public decimal AddonPriceMonthly { get; set; } = 0;
    public decimal AddonPriceYearly { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public bool IsBeta { get; set; } = false;
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public int DisplayOrder { get; set; } = 0;
}

public class UpdateModuleDto
{
    public string? ModuleName { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Version { get; set; }
    public string? Features { get; set; }
    public string? Doctypes { get; set; }
    public string? Dependencies { get; set; }
    public bool? IsFree { get; set; }
    public decimal? AddonPriceMonthly { get; set; }
    public decimal? AddonPriceYearly { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsBeta { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public int? DisplayOrder { get; set; }
}

public class ModuleDto : EntityDto<int>
{
    public string ModuleCode { get; set; }
    public string ModuleName { get; set; }
    public string? Description { get; set; }
    public string Category { get; set; }
    public string? FrappeAppName { get; set; }
    public string? GithubUrl { get; set; }
    public string? Version { get; set; }
    public string? Features { get; set; }
    public string? Doctypes { get; set; }
    public string? Dependencies { get; set; }
    public bool IsFree { get; set; }
    public decimal AddonPriceMonthly { get; set; }
    public decimal AddonPriceYearly { get; set; }
    public bool IsActive { get; set; }
    public bool IsBeta { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public int DisplayOrder { get; set; }
}
