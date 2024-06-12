using AutoMapper;
using SonEcommerce.ProductCategories;
using SonEcommerce.Products;
using SonEcommerce.Public.ProductCategories;
using SonEcommerce.Public.Products;
using SonEcommerce.Public.Users;
using SonEcommerce.Public.Manufacturers;
using SonEcommerce.Public.ProductAttributes;
using SonEcommerce.Manufacturers;
using SonEcommerce.Attributes;
using SonEcommerce.Orders;
using SonEcommerce.Public.Orders;
using SonEcommerce.Customers;
using Volo.Abp.Identity;

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

        //Order
        CreateMap<Order, OrderDto>();
        CreateMap<Order, OrderInListDto>();
        CreateMap<CreateOrderDto, Order>();

        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>();


        //Customer
        CreateMap<Customer, CustomerDto>();
        CreateMap<Customer, CustomerInListDto>();
        CreateMap<CreateUpdateCustomerDto, Customer>();

        //user
        CreateMap<IdentityUser, UserDto>();
        CreateMap<IdentityUser, UserInListDto>();

    }
}
