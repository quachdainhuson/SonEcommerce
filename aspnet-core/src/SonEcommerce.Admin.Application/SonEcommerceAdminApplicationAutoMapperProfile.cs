using AutoMapper;
using SonEcommerce.Admin.Manufacturers;
using SonEcommerce.Admin.ProductCategories;
using SonEcommerce.Admin.Products;
using SonEcommerce.Manufacturers;
using SonEcommerce.ProductCategories;
using SonEcommerce.Products;

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
    }
}
