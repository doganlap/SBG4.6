using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using AbpSaasPlatform.Modules;
using AbpSaasPlatform.Modules.Dtos;

namespace AbpSaasPlatform.Application.Modules;

public class ModuleAppService : ApplicationService, IModuleAppService
{
    private readonly IRepository<Module, int> _moduleRepository;

    public ModuleAppService(IRepository<Module, int> moduleRepository)
    {
        _moduleRepository = moduleRepository;
    }

    public async Task<ModuleDto> CreateAsync(CreateModuleDto input)
    {
        var module = new Module
        {
            ModuleCode = input.ModuleCode,
            ModuleName = input.ModuleName,
            Description = input.Description,
            Category = input.Category,
            FrappeAppName = input.FrappeAppName,
            GithubUrl = input.GithubUrl,
            Version = input.Version,
            Features = input.Features,
            Doctypes = input.Doctypes,
            Dependencies = input.Dependencies,
            IsFree = input.IsFree,
            AddonPriceMonthly = input.AddonPriceMonthly,
            AddonPriceYearly = input.AddonPriceYearly,
            IsActive = input.IsActive,
            IsBeta = input.IsBeta,
            Icon = input.Icon,
            Color = input.Color,
            DisplayOrder = input.DisplayOrder
        };

        await _moduleRepository.InsertAsync(module, autoSave: true);
        return ObjectMapper.Map<Module, ModuleDto>(module);
    }

    public async Task<ModuleDto> GetAsync(int id)
    {
        var module = await _moduleRepository.GetAsync(id);
        return ObjectMapper.Map<Module, ModuleDto>(module);
    }

    public async Task<ModuleDto> GetByCodeAsync(string moduleCode)
    {
        var module = await _moduleRepository.FirstOrDefaultAsync(x => x.ModuleCode == moduleCode);
        if (module == null)
        {
            throw new Exception($"Module with code '{moduleCode}' not found");
        }
        return ObjectMapper.Map<Module, ModuleDto>(module);
    }

    public async Task<List<ModuleDto>> GetListAsync(string? category = null, bool? isActive = null)
    {
        var query = await _moduleRepository.GetQueryableAsync();
        var modules = query.AsQueryable();
        
        if (category != null)
            modules = modules.Where(x => x.Category == category);
        if (isActive.HasValue)
            modules = modules.Where(x => x.IsActive == isActive.Value);

        var list = modules.OrderBy(x => x.DisplayOrder).ThenBy(x => x.ModuleName).ToList();
        return ObjectMapper.Map<List<Module>, List<ModuleDto>>(list);
    }

    public async Task<ModuleDto> UpdateAsync(int id, UpdateModuleDto input)
    {
        var module = await _moduleRepository.GetAsync(id);
        
        if (!string.IsNullOrEmpty(input.ModuleName))
            module.ModuleName = input.ModuleName;
        if (!string.IsNullOrEmpty(input.Description))
            module.Description = input.Description;
        if (!string.IsNullOrEmpty(input.Category))
            module.Category = input.Category;
        if (!string.IsNullOrEmpty(input.Version))
            module.Version = input.Version;
        if (input.Features != null)
            module.Features = input.Features;
        if (input.Doctypes != null)
            module.Doctypes = input.Doctypes;
        if (input.Dependencies != null)
            module.Dependencies = input.Dependencies;
        if (input.IsFree.HasValue)
            module.IsFree = input.IsFree.Value;
        if (input.AddonPriceMonthly.HasValue)
            module.AddonPriceMonthly = input.AddonPriceMonthly.Value;
        if (input.AddonPriceYearly.HasValue)
            module.AddonPriceYearly = input.AddonPriceYearly.Value;
        if (input.IsActive.HasValue)
            module.IsActive = input.IsActive.Value;
        if (input.IsBeta.HasValue)
            module.IsBeta = input.IsBeta.Value;
        if (input.Icon != null)
            module.Icon = input.Icon;
        if (input.Color != null)
            module.Color = input.Color;
        if (input.DisplayOrder.HasValue)
            module.DisplayOrder = input.DisplayOrder.Value;

        await _moduleRepository.UpdateAsync(module, autoSave: true);
        return ObjectMapper.Map<Module, ModuleDto>(module);
    }

    public async Task DeleteAsync(int id)
    {
        await _moduleRepository.DeleteAsync(id);
    }

    public async Task<List<ModuleDto>> GetActiveModulesAsync()
    {
        var modules = await _moduleRepository.GetListAsync(x => x.IsActive);
        return ObjectMapper.Map<List<Module>, List<ModuleDto>>(modules.OrderBy(x => x.DisplayOrder).ToList());
    }
}
