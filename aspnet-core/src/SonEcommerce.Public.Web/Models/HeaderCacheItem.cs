using SonEcommerce.Public.Manufacturers;
using SonEcommerce.Public.ProductCategories;
using SonEcommerce.Public.Products;
using System.Collections.Generic;
namespace SonEcommerce.Public.Web.Models
{
    public class HeaderCacheItem
    {
        public List<ProductCategoryInListDto> Categories { get; set; }
        public List<ManufacturerInListDto> Manufacturer { get; set; }

    }
}
