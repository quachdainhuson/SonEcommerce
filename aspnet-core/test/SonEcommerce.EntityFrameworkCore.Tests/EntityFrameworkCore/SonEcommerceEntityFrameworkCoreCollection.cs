using Xunit;

namespace SonEcommerce.EntityFrameworkCore;

[CollectionDefinition(SonEcommerceTestConsts.CollectionDefinitionName)]
public class SonEcommerceEntityFrameworkCoreCollection : ICollectionFixture<SonEcommerceEntityFrameworkCoreFixture>
{

}
