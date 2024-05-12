using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace SonEcommerce;

[Dependency(ReplaceServices = true)]
public class SonEcommerceBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "SonEcommerce";
}
