using SonEcommerce.Public.Products;

namespace SonEcommerce.Public.Web.Models
{
    public class CartItem
    {
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
