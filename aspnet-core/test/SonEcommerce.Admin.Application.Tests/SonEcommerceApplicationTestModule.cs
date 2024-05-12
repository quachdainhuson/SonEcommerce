using Volo.Abp.Modularity;

namespace SonEcommerce.Admin;

[DependsOn(
    typeof(SonEcommerceAdminApplicationModule),
    typeof(SonEcommerceDomainTestModule)
)]
public class SonEcommerceApplicationTestModule : AbpModule
{

}
