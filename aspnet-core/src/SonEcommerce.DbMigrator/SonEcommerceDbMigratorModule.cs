using SonEcommerce.Admin;
using SonEcommerce.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace SonEcommerce.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(SonEcommerceEntityFrameworkCoreModule),
    typeof(SonEcommerceApplicationContractsModule)
    )]
public class SonEcommerceDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "SonEcommerce:"; });
    }
}
