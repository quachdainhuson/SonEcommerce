using Volo.Abp.Modularity;

namespace SonEcommerce;

[DependsOn(
    typeof(SonEcommerceDomainModule),
    typeof(SonEcommerceTestBaseModule)
)]
public class SonEcommerceDomainTestModule : AbpModule
{

}
