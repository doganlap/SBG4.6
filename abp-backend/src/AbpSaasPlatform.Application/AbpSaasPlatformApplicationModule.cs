using Volo.Abp.Modularity;
using Volo.Abp.AutoMapper;
using AbpSaasPlatform.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace AbpSaasPlatform.Application;

[DependsOn(
    typeof(AbpSaasPlatformDomainModule),
    typeof(AbpAutoMapperModule)
)]
public class AbpSaasPlatformApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapper(
            typeof(AbpSaasPlatformApplicationModule).Assembly
        );
    }
}
