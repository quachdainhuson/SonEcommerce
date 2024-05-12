using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SonEcommerce.Data;

/* This is used if database provider does't define
 * ISonEcommerceDbSchemaMigrator implementation.
 */
public class NullSonEcommerceDbSchemaMigrator : ISonEcommerceDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
