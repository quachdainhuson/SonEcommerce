using Volo.Abp.Modularity;

namespace SonEcommerce.Public;

[DependsOn(
    typeof(SonEcommercePublicApplicationModule),
    typeof(SonEcommerceDomainTestModule)
)]
public class SonEcommercePublicApplicationTestModule : AbpModule
{

}
