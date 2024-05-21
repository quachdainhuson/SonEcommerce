using SonEcommerce.Public.ProductCategories;
using SonEcommerce.Public.Products;
using System.Collections.Generic;
namespace SonEcommerce.Public.Web.Models
{
    public class HomeCacheItem
    {
        public List<ProductCategoryInListDto> Categories { get; set; }
        public List<ProductInListDto> TopSellerProduct { get; set; }
    }
}
