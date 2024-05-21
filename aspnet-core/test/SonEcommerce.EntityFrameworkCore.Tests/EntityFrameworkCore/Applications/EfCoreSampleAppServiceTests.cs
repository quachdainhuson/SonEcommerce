using SonEcommerce.Admin.Samples;
using SonEcommerce.Samples;
using Xunit;

namespace SonEcommerce.EntityFrameworkCore.Applications;

[Collection(SonEcommerceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<SonEcommerceEntityFrameworkCoreTestModule>
{

}
