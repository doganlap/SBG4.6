using Volo.Abp.Modularity;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using AbpSaasPlatform.Domain;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace AbpSaasPlatform.EntityFrameworkCore;

[DependsOn(
    typeof(AbpSaasPlatformDomainModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule)
)]
public class AbpSaasPlatformEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AbpSaasPlatformDbContext>(options =>
        {
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL();
        });
    }
}
