using AutoMapper;
using SonEcommerce.Admin.Manufacturers;
using SonEcommerce.Admin.ProductAttributes;
using SonEcommerce.Admin.ProductCategories;
using SonEcommerce.Admin.Products;
using SonEcommerce.Admin.Roles;
using SonEcommerce.Attributes;
using SonEcommerce.Manufacturers;
using SonEcommerce.ProductCategories;
using SonEcommerce.Products;
using SonEcommerce.Roles;
using Volo.Abp.Identity;

namespace SonEcommerce.Admin;

public class SonEcommerceAdminApplicationAutoMapperProfile : Profile
{
    public SonEcommerceAdminApplicationAutoMapperProfile()
    {
        //Product Category
        CreateMap<ProductCategory, ProductCategoryDto>();
        CreateMap<ProductCategory, ProductCategoryInListDto>();
        CreateMap<CreateUpdateProductCategoryDto, ProductCategory>();

        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductInListDto>();
        CreateMap<CreateUpdateProductDto, Product>();
        
        CreateMap<Manufacturer, ManufacturerDto>();
        CreateMap<Manufacturer, ManufacturerInListDto>();
        CreateMap<CreateUpdateManufacturerDto, Manufacturer>();

        CreateMap<ProductAttribute, ProductAttributeDto>();
        CreateMap<ProductAttribute, ProductAttributeInListDto>();
        CreateMap<CreateUpdateProductAttributeDto, ProductAttribute>();

        CreateMap<IdentityRole, RoleDto>().ForMember(x => x.Description,
            map => map.MapFrom(x => x.ExtraProperties.ContainsKey(RoleConsts.DescriptionFieldName)
            ?
            x.ExtraProperties[RoleConsts.DescriptionFieldName] : null)); ;
        CreateMap<IdentityRole, RoleInListDto>().ForMember(x=> x.Description, 
            map=> map.MapFrom(x=>x.ExtraProperties.ContainsKey(RoleConsts.DescriptionFieldName)
            ?
            x.ExtraProperties[RoleConsts.DescriptionFieldName] : null));
        CreateMap<CreateUpdateRoleDto, IdentityRole>();
    }
}
