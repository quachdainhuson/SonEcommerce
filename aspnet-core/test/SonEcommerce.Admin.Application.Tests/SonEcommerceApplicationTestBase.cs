using Volo.Abp.Modularity;

namespace SonEcommerce.Admin;

public abstract class SonEcommerceApplicationTestBase<TStartupModule> : SonEcommerceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
