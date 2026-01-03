using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;

namespace AbpSaasPlatform.Domain;

[DependsOn(
    typeof(AbpSaasPlatformDomainSharedModule),
    typeof(AbpIdentityDomainModule),
    typeof(AbpTenantManagementDomainModule))]
public class AbpSaasPlatformDomainModule : AbpModule
{
}
