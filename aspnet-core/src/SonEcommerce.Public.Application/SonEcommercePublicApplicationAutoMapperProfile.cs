using AutoMapper;
using SonEcommerce.ProductCategories;
using SonEcommerce.Products;
using SonEcommerce.Public.ProductCategories;
using SonEcommerce.Public.Products;
using SonEcommerce.Public.Manufacturers;
using SonEcommerce.Public.ProductAttributes;
using SonEcommerce.Manufacturers;
using SonEcommerce.Attributes;

namespace SonEcommerce.Public;

public class SonEcommercePublicApplicationAutoMapperProfile : Profile
{
    public SonEcommercePublicApplicationAutoMapperProfile()
    {
        //Product Category
        CreateMap<ProductCategory, ProductCategoryDto>();
        CreateMap<ProductCategory, ProductCategoryInListDto>();

        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductInListDto>();

        CreateMap<Manufacturer, ManufacturerDto>();
        CreateMap<Manufacturer, ManufacturerInListDto>();

        CreateMap<ProductAttribute, ProductAttributeDto>();
        CreateMap<ProductAttribute, ProductAttributeInListDto>();
    }
}
