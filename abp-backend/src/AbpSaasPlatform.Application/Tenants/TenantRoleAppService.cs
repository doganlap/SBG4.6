using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AbpSaasPlatform.Tenants;
using AbpSaasPlatform.Tenants.Dtos;

namespace AbpSaasPlatform.Application.Tenants;

public class TenantRoleAppService : ApplicationService, ITenantRoleAppService
{
    private readonly IRepository<TenantRole, int> _roleRepository;

    public TenantRoleAppService(IRepository<TenantRole, int> roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<TenantRoleDto> CreateAsync(CreateTenantRoleDto input)
    {
        var role = new TenantRole
        {
            RoleCode = input.RoleCode,
            RoleName = input.RoleName,
            Description = input.Description,
            Permissions = input.Permissions,
            AllowedModules = input.AllowedModules,
            Level = input.Level,
            ParentRoleId = input.ParentRoleId,
            IsSystemRole = input.IsSystemRole,
            IsActive = input.IsActive
        };

        await _roleRepository.InsertAsync(role, autoSave: true);
        return ObjectMapper.Map<TenantRole, TenantRoleDto>(role);
    }

    public async Task<TenantRoleDto> GetAsync(int id)
    {
        var role = await _roleRepository.GetAsync(id);
        return ObjectMapper.Map<TenantRole, TenantRoleDto>(role);
    }

    public async Task<TenantRoleDto> GetByCodeAsync(string roleCode)
    {
        var role = await _roleRepository.FirstOrDefaultAsync(x => x.RoleCode == roleCode);
        if (role == null)
        {
            throw new Exception($"Role with code '{roleCode}' not found");
        }
        return ObjectMapper.Map<TenantRole, TenantRoleDto>(role);
    }

    public async Task<List<TenantRoleDto>> GetListAsync(bool? isActive = null)
    {
        var query = await _roleRepository.GetQueryableAsync();
        var roles = isActive.HasValue 
            ? query.Where(x => x.IsActive == isActive.Value)
            : query;
        
        var list = roles.OrderByDescending(x => x.Level).ThenBy(x => x.RoleName).ToList();
        return ObjectMapper.Map<List<TenantRole>, List<TenantRoleDto>>(list);
    }

    public async Task<TenantRoleDto> UpdateAsync(int id, UpdateTenantRoleDto input)
    {
        var role = await _roleRepository.GetAsync(id);
        
        if (role.IsSystemRole)
        {
            throw new Exception("Cannot update system role");
        }
        
        if (!string.IsNullOrEmpty(input.RoleName))
            role.RoleName = input.RoleName;
        if (!string.IsNullOrEmpty(input.Description))
            role.Description = input.Description;
        if (input.Permissions != null)
            role.Permissions = input.Permissions;
        if (input.AllowedModules != null)
            role.AllowedModules = input.AllowedModules;
        if (input.Level.HasValue)
            role.Level = input.Level.Value;
        if (input.ParentRoleId.HasValue)
            role.ParentRoleId = input.ParentRoleId.Value;
        if (input.IsActive.HasValue)
            role.IsActive = input.IsActive.Value;

        await _roleRepository.UpdateAsync(role, autoSave: true);
        return ObjectMapper.Map<TenantRole, TenantRoleDto>(role);
    }

    public async Task DeleteAsync(int id)
    {
        var role = await _roleRepository.GetAsync(id);
        if (role.IsSystemRole)
        {
            throw new Exception("Cannot delete system role");
        }
        await _roleRepository.DeleteAsync(id);
    }
}
