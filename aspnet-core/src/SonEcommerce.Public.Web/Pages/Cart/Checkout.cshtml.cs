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
using Volo.Abp.TextTemplating;
using Volo.Abp.Emailing;
using SonEcommerce.Emailing;


namespace SonEcommerce.Public.Web.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        private readonly OrdersAppService _ordersAppService;
        private readonly UsersAppService _usersAppService;
        private readonly ProductsAppService _productsAppService;
        private readonly IEmailSender _emailSender;
        private readonly ITemplateRenderer _templateRenderer;

        public CheckoutModel(OrdersAppService ordersAppService, UsersAppService usersAppService, ProductsAppService productsAppService, IEmailSender emailSender, ITemplateRenderer templateRenderer)
        {
            _ordersAppService = ordersAppService;
            _usersAppService = usersAppService;
            _productsAppService = productsAppService;
            _emailSender = emailSender;
            _templateRenderer = templateRenderer;

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
            Order.UserCity = Order.UserCity.Trim();
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                CurrentUser = await _usersAppService.GetUserByIdAsync(userId);

            }
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
                UserCity = Order.UserCity,
                UserDistrict = Order.UserDistrict,
                UserWard = Order.UserWard,
                CustomerUserId = currentUserId,
                Items = cartItems

            });
            CartItems = GetCartItems();

            if (order != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var email = User.GetSpecificClaim(ClaimTypes.Email);
                    var emailBody = await _templateRenderer.RenderAsync(
                        EmailTemplates.CreateOrderEmail,
                        new
                        {
                            message = "Tạo đơn hàng thành công!!",
                            orderEmail = order

                        });
                    await _emailSender.SendAsync(email, "Tạo đơn hàng thành công", emailBody);
                }

                CreateStatus = true;
                HttpContext.Session.Remove(SonEcommerceConsts.Cart);
                CartItems.Clear();
            }
            else
            {
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
