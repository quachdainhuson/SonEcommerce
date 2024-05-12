using Volo.Abp.Modularity;

namespace SonEcommerce;

[DependsOn(
    typeof(SonEcommerceApplicationModule),
    typeof(SonEcommerceDomainTestModule)
)]
public class SonEcommerceApplicationTestModule : AbpModule
{

}
