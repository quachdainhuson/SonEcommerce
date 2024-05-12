using SonEcommerce.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SonEcommerce.Permissions;

public class SonEcommercePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SonEcommercePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(SonEcommercePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SonEcommerceResource>(name);
    }
}
