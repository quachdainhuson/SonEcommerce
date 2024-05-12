using Volo.Abp.Modularity;

namespace SonEcommerce;

public abstract class SonEcommerceApplicationTestBase<TStartupModule> : SonEcommerceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
