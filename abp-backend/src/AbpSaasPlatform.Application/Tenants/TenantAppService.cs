using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AbpSaasPlatform.Tenants;
using AbpSaasPlatform.Tenants.Dtos;
using AutoMapper;

namespace AbpSaasPlatform.Application.Tenants;

public class TenantAppService : ApplicationService, ITenantAppService
{
    private readonly IRepository<Tenant, Guid> _tenantRepository;

    public TenantAppService(IRepository<Tenant, Guid> tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<TenantDto> CreateAsync(TenantCreateDto input)
    {
        var tenant = new Tenant(GuidGenerator.Create())
        {
            Name = input.Name,
            Domain = input.Domain,
            Plan = input.Plan,
            Status = "provisioning"
        };
        await _tenantRepository.InsertAsync(tenant);
        return ObjectMapper.Map<Tenant, TenantDto>(tenant);
    }

    public async Task<TenantDto> GetAsync(Guid id)
    {
        var tenant = await _tenantRepository.GetAsync(id);
        return ObjectMapper.Map<Tenant, TenantDto>(tenant);
    }

    public async Task<List<TenantDto>> GetListAsync()
    {
        var tenants = await _tenantRepository.GetListAsync();
        return ObjectMapper.Map<List<Tenant>, List<TenantDto>>(tenants);
    }

    public async Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input)
    {
        var tenant = await _tenantRepository.GetAsync(id);
        tenant.Name = input.Name;
        tenant.Plan = input.Plan;
        tenant.MaxUsers = input.MaxUsers;
        tenant.MaxStorage = input.MaxStorage;
        await _tenantRepository.UpdateAsync(tenant);
        return ObjectMapper.Map<Tenant, TenantDto>(tenant);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _tenantRepository.DeleteAsync(id);
    }
}
