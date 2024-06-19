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
using Microsoft.AspNetCore.Identity;
using SonEcommerce.Emailing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.TextTemplating;
using Volo.Abp.Identity;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
using Volo.Abp.Emailing;

namespace SonEcommerce.Public.Web.Pages.User
{
    public class AccountModel : PageModel
    {
        private readonly UsersAppService _usersAppService;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IEmailSender _emailSender;
        private readonly ITemplateRenderer _templateRenderer;
        private readonly OrdersAppService _ordersAppService;
        [BindProperty]
        public UpdateUserDto UpdateUser { get; set; }

        public UserDto CurrentUser { get; set; }
        public List<OrderInListDto> Orders { get; set; }
        public OrderItemDto OrderItem { get; set; }
        [BindProperty]
        public string userId { get; set; }
        [BindProperty]
        public string StatusMessage { get; set; }
        public AccountModel(UsersAppService usersAppService, OrdersAppService ordersAppService, IdentityUserManager identityUserManager, IEmailSender emailSender, ITemplateRenderer templateRenderer)
        {
            _usersAppService = usersAppService;
            _ordersAppService = ordersAppService;
            _identityUserManager = identityUserManager;

            _emailSender = emailSender;
            _templateRenderer = templateRenderer;

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
        public async Task<IActionResult> OnPostSendEmailConfirmationAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await SendEmailConfirmAsync(userId);
                StatusMessage = "Email xác thực đã được gửi. Vui lòng kiểm tra email của bạn.";
            }
            return RedirectToPage();
        }

        private async Task SendEmailConfirmAsync(string userId)
        {
            var user = await _identityUserManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(Microsoft.AspNetCore.Identity.IdentityUser), userId);
            }

            var code = await _identityUserManager.GenerateEmailConfirmationTokenAsync(user);

            // Đường dẫn xác thực email
            var confirmationLink = Url.Page(
                EmailTemplates.ConfirmEmail,
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);

            // Chuẩn bị dữ liệu cho template
            var model = new
            {
                ConfirmationLink = confirmationLink
            };

            // Render template
            var emailBody = await _templateRenderer.RenderAsync(EmailTemplates.ConfirmEmail, model);

            // Gửi email
            await _emailSender.SendAsync(user.Email, "Xác thực tài khoản", emailBody);
        }


    }


}
