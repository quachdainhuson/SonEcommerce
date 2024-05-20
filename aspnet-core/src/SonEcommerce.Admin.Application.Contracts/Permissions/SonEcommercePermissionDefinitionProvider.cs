using SonEcommerce.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SonEcommerce.Admin.Permissions;

public class SonEcommercePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        //System
        var catalogGroup = context.AddGroup(SonEcommercePermissions.CatalogGroupName, L("Permission:Catalog"));

        // Manufacturer
        var manufacturerPermission = catalogGroup.AddPermission(SonEcommercePermissions.Manufacturer.Default, L("Permission:Catalog.Manufacturer"));
        manufacturerPermission.AddChild(SonEcommercePermissions.Manufacturer.Create, L("Permission:Catalog.Manufacturer.Create"));
        manufacturerPermission.AddChild(SonEcommercePermissions.Manufacturer.Update, L("Permission:Catalog.Manufacturer.Update"));
        manufacturerPermission.AddChild(SonEcommercePermissions.Manufacturer.Delete, L("Permission:Catalog.Manufacturer.Delete"));

        // productCategory
        var productCategoryPermission = catalogGroup.AddPermission(SonEcommercePermissions.ProductCategory.Default, L("Permission:Catalog.ProductCategory"));
        productCategoryPermission.AddChild(SonEcommercePermissions.ProductCategory.Create, L("Permission:Catalog.ProductCategory.Create"));
        productCategoryPermission.AddChild(SonEcommercePermissions.ProductCategory.Update, L("Permission:Catalog.ProductCategory.Update"));
        productCategoryPermission.AddChild(SonEcommercePermissions.ProductCategory.Delete, L("Permission:Catalog.ProductCategory.Delete"));
        //Add product
        var productPermission = catalogGroup.AddPermission(SonEcommercePermissions.Product.Default, L("Permission:Catalog.Product"));
        productPermission.AddChild(SonEcommercePermissions.Product.Create, L("Permission:Catalog.Product.Create"));
        productPermission.AddChild(SonEcommercePermissions.Product.Update, L("Permission:Catalog.Product.Update"));
        productPermission.AddChild(SonEcommercePermissions.Product.Delete, L("Permission:Catalog.Product.Delete"));
        productPermission.AddChild(SonEcommercePermissions.Product.AttributeManage, L("Permission:Catalog.Product.AttributeManage"));

        //Add attribute
        var attributePermission = catalogGroup.AddPermission(SonEcommercePermissions.Attribute.Default, L("Permission:Catalog.Attribute"));
        attributePermission.AddChild(SonEcommercePermissions.Attribute.Create, L("Permission:Catalog.Attribute.Create"));
        attributePermission.AddChild(SonEcommercePermissions.Attribute.Update, L("Permission:Catalog.Attribute.Update"));
        attributePermission.AddChild(SonEcommercePermissions.Attribute.Delete, L("Permission:Catalog.Attribute.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SonEcommerceResource>(name);
    }
}
