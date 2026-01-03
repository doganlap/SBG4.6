using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AbpSaasPlatform.Tenants;
using AbpSaasPlatform.Tenants.Dtos;

namespace AbpSaasPlatform.Application.Tenants;

public class TenantUserAppService : ApplicationService, ITenantUserAppService
{
    private readonly IRepository<TenantUser, Guid> _userRepository;

    public TenantUserAppService(IRepository<TenantUser, Guid> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<TenantUserDto> CreateAsync(CreateTenantUserDto input)
    {
        var user = new TenantUser(GuidGenerator.Create())
        {
            UserId = $"USR{GuidGenerator.Create().ToString("N")[..8].ToUpper()}",
            Email = input.Email,
            PasswordHash = input.PasswordHash,
            FirstName = input.FirstName,
            LastName = input.LastName,
            Phone = input.Phone,
            Mobile = input.Mobile,
            AvatarUrl = input.AvatarUrl,
            EmployeeId = input.EmployeeId,
            Department = input.Department,
            Designation = input.Designation,
            ReportsTo = input.ReportsTo,
            RoleId = input.RoleId,
            IsAdmin = input.IsAdmin,
            Permissions = input.Permissions,
            AllowedModules = input.AllowedModules,
            IsActive = input.IsActive,
            IsVerified = false,
            TwoFactorEnabled = false,
            FailedLoginAttempts = 0
        };

        await _userRepository.InsertAsync(user, autoSave: true);
        return ObjectMapper.Map<TenantUser, TenantUserDto>(user);
    }

    public async Task<TenantUserDto> GetAsync(Guid id)
    {
        var user = await _userRepository.GetAsync(id);
        return ObjectMapper.Map<TenantUser, TenantUserDto>(user);
    }

    public async Task<TenantUserDto> GetByEmailAsync(string email)
    {
        var user = await _userRepository.FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
        {
            throw new Exception($"User with email '{email}' not found");
        }
        return ObjectMapper.Map<TenantUser, TenantUserDto>(user);
    }

    public async Task<List<TenantUserDto>> GetListAsync()
    {
        var users = await _userRepository.GetListAsync();
        return ObjectMapper.Map<List<TenantUser>, List<TenantUserDto>>(users.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList());
    }

    public async Task<List<TenantUserDto>> GetByDepartmentAsync(string department)
    {
        var users = await _userRepository.GetListAsync(x => x.Department == department && x.IsActive);
        return ObjectMapper.Map<List<TenantUser>, List<TenantUserDto>>(users.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList());
    }

    public async Task<TenantUserDto> UpdateAsync(Guid id, UpdateTenantUserDto input)
    {
        var user = await _userRepository.GetAsync(id);
        
        if (!string.IsNullOrEmpty(input.FirstName))
            user.FirstName = input.FirstName;
        if (!string.IsNullOrEmpty(input.LastName))
            user.LastName = input.LastName;
        if (input.Phone != null)
            user.Phone = input.Phone;
        if (input.Mobile != null)
            user.Mobile = input.Mobile;
        if (input.AvatarUrl != null)
            user.AvatarUrl = input.AvatarUrl;
        if (input.EmployeeId != null)
            user.EmployeeId = input.EmployeeId;
        if (input.Department != null)
            user.Department = input.Department;
        if (input.Designation != null)
            user.Designation = input.Designation;
        if (input.ReportsTo.HasValue)
            user.ReportsTo = input.ReportsTo.Value;
        if (input.RoleId.HasValue)
            user.RoleId = input.RoleId.Value;
        if (input.IsAdmin.HasValue)
            user.IsAdmin = input.IsAdmin.Value;
        if (input.Permissions != null)
            user.Permissions = input.Permissions;
        if (input.AllowedModules != null)
            user.AllowedModules = input.AllowedModules;
        if (input.IsActive.HasValue)
            user.IsActive = input.IsActive.Value;

        await _userRepository.UpdateAsync(user, autoSave: true);
        return ObjectMapper.Map<TenantUser, TenantUserDto>(user);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _userRepository.DeleteAsync(id);
    }

    public async Task<TenantUserDto> ActivateAsync(Guid id)
    {
        var user = await _userRepository.GetAsync(id);
        user.IsActive = true;
        await _userRepository.UpdateAsync(user, autoSave: true);
        return ObjectMapper.Map<TenantUser, TenantUserDto>(user);
    }

    public async Task<TenantUserDto> DeactivateAsync(Guid id)
    {
        var user = await _userRepository.GetAsync(id);
        user.IsActive = false;
        await _userRepository.UpdateAsync(user, autoSave: true);
        return ObjectMapper.Map<TenantUser, TenantUserDto>(user);
    }

    public async Task<TenantUserDto> UpdateLastLoginAsync(Guid id, string ipAddress)
    {
        var user = await _userRepository.GetAsync(id);
        user.LastLoginAt = DateTime.UtcNow;
        user.LastLoginIp = ipAddress;
        user.LastActivityAt = DateTime.UtcNow;
        user.FailedLoginAttempts = 0;
        user.LockedUntil = null;
        
        await _userRepository.UpdateAsync(user, autoSave: true);
        return ObjectMapper.Map<TenantUser, TenantUserDto>(user);
    }
}
