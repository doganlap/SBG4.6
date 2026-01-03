using System;
using Volo.Abp.Application.Dtos;

namespace AbpSaasPlatform.Tenants.Dtos;

public class CreateTenantUserDto
{
    public string Email { get; set; }
    public string? PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? AvatarUrl { get; set; }
    public string? EmployeeId { get; set; }
    public string? Department { get; set; }
    public string? Designation { get; set; }
    public Guid? ReportsTo { get; set; }
    public int? RoleId { get; set; }
    public bool IsAdmin { get; set; } = false;
    public string? Permissions { get; set; }
    public string? AllowedModules { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateTenantUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? AvatarUrl { get; set; }
    public string? EmployeeId { get; set; }
    public string? Department { get; set; }
    public string? Designation { get; set; }
    public Guid? ReportsTo { get; set; }
    public int? RoleId { get; set; }
    public bool? IsAdmin { get; set; }
    public string? Permissions { get; set; }
    public string? AllowedModules { get; set; }
    public bool? IsActive { get; set; }
}

public class TenantUserDto : EntityDto<Guid>
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? AvatarUrl { get; set; }
    public string? EmployeeId { get; set; }
    public string? Department { get; set; }
    public string? Designation { get; set; }
    public Guid? ReportsTo { get; set; }
    public int? RoleId { get; set; }
    public bool IsAdmin { get; set; }
    public string? Permissions { get; set; }
    public string? AllowedModules { get; set; }
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? LastLoginIp { get; set; }
    public DateTime? LastActivityAt { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTime? LockedUntil { get; set; }
    public string? ErpnextUserId { get; set; }
    public string? ErpnextUserEmail { get; set; }
}
