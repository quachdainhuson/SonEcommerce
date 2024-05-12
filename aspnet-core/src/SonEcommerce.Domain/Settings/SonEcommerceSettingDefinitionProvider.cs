using Volo.Abp.Settings;

namespace SonEcommerce.Settings;

public class SonEcommerceSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(SonEcommerceSettings.MySetting1));
    }
}
