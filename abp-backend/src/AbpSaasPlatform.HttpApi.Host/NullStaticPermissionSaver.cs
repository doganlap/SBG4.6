using System.Threading.Tasks;
using Volo.Abp.PermissionManagement;

namespace AbpSaasPlatform.HttpApi.Host;

public class NullStaticPermissionSaver : IStaticPermissionSaver
{
    public Task SaveAsync()
    {
        return Task.CompletedTask;
    }
}