using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using AbpSaasPlatform.Modules.Dtos;

namespace AbpSaasPlatform.Modules;

public interface IModuleAppService : IApplicationService
{
    Task<ModuleDto> CreateAsync(CreateModuleDto input);
    Task<ModuleDto> GetAsync(int id);
    Task<ModuleDto> GetByCodeAsync(string moduleCode);
    Task<List<ModuleDto>> GetListAsync(string? category = null, bool? isActive = null);
    Task<ModuleDto> UpdateAsync(int id, UpdateModuleDto input);
    Task DeleteAsync(int id);
    Task<List<ModuleDto>> GetActiveModulesAsync();
}
