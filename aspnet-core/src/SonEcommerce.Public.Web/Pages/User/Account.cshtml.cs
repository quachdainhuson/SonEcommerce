using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SonEcommerce.Public.Users;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Users;
using SonEcommerce.Public.Orders;
using Volo.Abp;

namespace SonEcommerce.Public.Web.Pages.User
{
    public class AccountModel : PageModel
    {
        private readonly UsersAppService _usersAppService;
        private readonly OrdersAppService _ordersAppService;
        [BindProperty]
        public UpdateUserDto UpdateUser { get; set; }

        public UserDto CurrentUser { get; set; }
        public List<OrderInListDto> Orders { get; set; }
        public OrderItemDto OrderItem { get; set; }
        [BindProperty]
        public string userId { get; set; }
        public AccountModel(UsersAppService usersAppService, OrdersAppService ordersAppService)
        {
            _usersAppService = usersAppService;
            _ordersAppService = ordersAppService;

        }

        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                CurrentUser = await _usersAppService.GetUserByIdAsync(userId);
                var orders = await _ordersAppService.GetListOrderByUserIdAsync(Guid.Parse(userId));
                Orders = orders;

            }


        }
        //lấy thông tin đơn hàng theo user id

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    CurrentUser = await _usersAppService.GetUserByIdAsync(userId);
                    var orders = await _ordersAppService.GetListOrderByUserIdAsync(Guid.Parse(userId));
                    Orders = orders;
                    await _usersAppService.UpdateAsync(Guid.Parse(userId), UpdateUser);

                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UpdateUser.PhoneNumber", ex.Message);
                ModelState.AddModelError("UpdateError", ex.Message);
                return Page();
            }


        }


    }


}
