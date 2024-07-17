using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using SonEcommerce.Public.Products;
using SonEcommerce.Public.Web.Models;
using Volo.Abp;

namespace SonEcommerce.Public.Web.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly IProductsAppService _productsAppService;
        public IndexModel(IProductsAppService productsAppService)
        {
            _productsAppService = productsAppService;
        }

        [BindProperty]
        public List<CartItem> CartItems { get; set; }
        public string categorySlug { get; set; }
        public string slug { get; set; }
        public async Task<IActionResult> OnGetAsync(string action, string id, int quantity)
        {
            try
            {
                if (quantity == 0)
                {
                    quantity = 1;
                }
                var cart = HttpContext.Session.GetString(SonEcommerceConsts.Cart);
                var productCarts = new Dictionary<string, CartItem>();
                if (cart != null)
                {
                    productCarts = JsonSerializer.Deserialize<Dictionary<string, CartItem>>(cart);

                    // Kiểm tra trạng thái của từng sản phẩm trong giỏ hàng
                    var keysToRemove = new List<string>();
                    foreach (var item in productCarts)
                    {
                        var product = await _productsAppService.GetProductByIdsAsync(Guid.Parse(item.Key));
                        if (product.IsActive == false)
                        {
                            keysToRemove.Add(item.Key);
                        }
                    }

                    // Xóa các sản phẩm không hoạt động khỏi giỏ hàng
                    foreach (var key in keysToRemove)
                    {
                        productCarts.Remove(key);
                    }

                    HttpContext.Session.SetString(SonEcommerceConsts.Cart, JsonSerializer.Serialize(productCarts));
                }

                if (!string.IsNullOrEmpty(action))
                {
                    if (action == "add")
                    {
                        var product = await _productsAppService.GetProductByIdsAsync(Guid.Parse(id));
                        if (product.IsActive == false)
                        {
                            categorySlug = product.CategorySlug;
                            slug = product.Slug;
                            throw new UserFriendlyException("Sản phẩm này hiện đang không hoạt động!");
                        }
                        if (productCarts.ContainsKey(id))
                        {
                            productCarts[id].Quantity += quantity;
                        }
                        else
                        {
                            productCarts.Add(id, new CartItem()
                            {
                                Product = product,
                                Quantity = quantity
                            });
                        }
                        HttpContext.Session.SetString(SonEcommerceConsts.Cart, JsonSerializer.Serialize(productCarts));
                    }
                    else if (action == "remove")
                    {
                        if (productCarts.ContainsKey(id))
                        {
                            productCarts.Remove(id);
                        }
                        HttpContext.Session.SetString(SonEcommerceConsts.Cart, JsonSerializer.Serialize(productCarts));
                    }
                }
                CartItems = productCarts.Values.ToList();
                return Page();
            }
            catch (Exception ex)
            {
                TempData["MessageError"] = ex.Message;
                // trả về trang này "/products/{categorySlug}/{slug}.html"
                return Redirect($"/products/{categorySlug}/{slug}.html");
            }
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var cart = HttpContext.Session.GetString(SonEcommerceConsts.Cart);
            var productCarts = JsonSerializer.Deserialize<Dictionary<string, CartItem>>(cart);
            foreach (var item in productCarts)
            {
                var cartItem = CartItems.FirstOrDefault(x => x.Product.Id == item.Value.Product.Id);
                cartItem.Product = await _productsAppService.GetProductByIdsAsync(cartItem.Product.Id);
                item.Value.Quantity = cartItem != null ? cartItem.Quantity : 0;
            }

            HttpContext.Session.SetString(SonEcommerceConsts.Cart, JsonSerializer.Serialize(productCarts));
            return Redirect("/shop-cart.html");
        }
    }
}
