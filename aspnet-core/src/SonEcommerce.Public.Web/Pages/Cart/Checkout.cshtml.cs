using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SonEcommerce.Public.Orders;
using SonEcommerce.Public.Web.Extensions;
using SonEcommerce.Public.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Users;
using SonEcommerce.Public.Users;
using SonEcommerce.Public.Products;


namespace SonEcommerce.Public.Web.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        private readonly OrdersAppService _ordersAppService;
        private readonly UsersAppService _usersAppService;

        public CheckoutModel(OrdersAppService ordersAppService, UsersAppService usersAppService)
        {
            _ordersAppService = ordersAppService;
            _usersAppService = usersAppService;
        }
        public List<CartItem> CartItems { get; set; }
        public UserDto CurrentUser { get; set; }

        public bool? CreateStatus { set; get; }

        [BindProperty]
        public OrderDto Order { set; get; }
        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                CurrentUser = await _usersAppService.GetUserByIdAsync(userId);
                CartItems = GetCartItems();

            }
            

        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid == false)
            {

            }
            var cartItems = new List<OrderItemDto>();
            foreach (var item in GetCartItems())
            {
                cartItems.Add(new OrderItemDto()
                {
                    Price = item.Product.SellPrice,
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity
                });
            }
            Guid? currentUserId = User.Identity.IsAuthenticated ? User.GetUserId() : null;
            var order = await _ordersAppService.CreateAsync(new CreateOrderDto()
            {
                CustomerName = Order.CustomerName,
                CustomerAddress = Order.CustomerAddress,
                CustomerPhoneNumber = Order.CustomerPhoneNumber,
                CustomerUserId = currentUserId,
                Items = cartItems

            });
            CartItems = GetCartItems();

            if (order != null) {
                CreateStatus = true;
                HttpContext.Session.Remove(SonEcommerceConsts.Cart);
                CartItems.Clear();

            }

            else {
                CreateStatus = false;

            }
        }

        private List<CartItem> GetCartItems()
        {
            var cart = HttpContext.Session.GetString(SonEcommerceConsts.Cart);
            var productCarts = new Dictionary<string, CartItem>();
            if (cart != null)
            {
                productCarts = JsonSerializer.Deserialize<Dictionary<string, CartItem>>(cart);
            }
            return productCarts.Values.ToList();
        }

    }
}
