using SonEcommerce.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SonEcommerce.Public.Permissions;

public class SonEcommercePublicPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SonEcommercePublicPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(PublicPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SonEcommerceResource>(name);
    }
}
