using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Tenants.Dtos;

public class CreateTenantRoleDto
{
    public string RoleCode { get; set; }
    public string RoleName { get; set; }
    public string? Description { get; set; }
    public string? Permissions { get; set; }
    public string? AllowedModules { get; set; }
    public int Level { get; set; } = 0;
    public int? ParentRoleId { get; set; }
    public bool IsSystemRole { get; set; } = false;
    public bool IsActive { get; set; } = true;
}

public class UpdateTenantRoleDto
{
    public string? RoleName { get; set; }
    public string? Description { get; set; }
    public string? Permissions { get; set; }
    public string? AllowedModules { get; set; }
    public int? Level { get; set; }
    public int? ParentRoleId { get; set; }
    public bool? IsActive { get; set; }
}

public class TenantRoleDto : EntityDto<int>
{
    public string RoleCode { get; set; }
    public string RoleName { get; set; }
    public string? Description { get; set; }
    public string? Permissions { get; set; }
    public string? AllowedModules { get; set; }
    public int Level { get; set; }
    public int? ParentRoleId { get; set; }
    public bool IsSystemRole { get; set; }
    public bool IsActive { get; set; }
}
