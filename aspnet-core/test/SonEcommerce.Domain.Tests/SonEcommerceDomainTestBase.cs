using Volo.Abp.Modularity;

namespace SonEcommerce;

/* Inherit from this class for your domain layer tests. */
public abstract class SonEcommerceDomainTestBase<TStartupModule> : SonEcommerceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
