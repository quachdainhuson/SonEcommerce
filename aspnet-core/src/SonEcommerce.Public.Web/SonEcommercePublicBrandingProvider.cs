using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace SonEcommerce.Public.Web;

[Dependency(ReplaceServices = true)]
public class SonEcommercePublicBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Public";
}
