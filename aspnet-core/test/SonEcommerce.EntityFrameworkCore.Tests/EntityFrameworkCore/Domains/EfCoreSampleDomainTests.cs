using SonEcommerce.Samples;
using Xunit;

namespace SonEcommerce.EntityFrameworkCore.Domains;

[Collection(SonEcommerceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<SonEcommerceEntityFrameworkCoreTestModule>
{

}
