﻿using Microsoft.AspNetCore.Mvc;
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
using Microsoft.EntityFrameworkCore;

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
        [BindProperty]
        public bool ShowOTPVerification { get; set; }
        [BindProperty]
        public string OTP { get; set; }
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
        public async Task OnPostSendEmailConfirmationAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                CurrentUser = await _usersAppService.GetUserByIdAsync(userId);
                var orders = await _ordersAppService.GetListOrderByUserIdAsync(Guid.Parse(userId));
                Orders = orders;
                await SendEmailConfirmAsync(userId);
                StatusMessage = "Email xác thực OTP đã được gửi. Vui lòng kiểm tra email của bạn.";
                TempData["ShowOTP"] = true;
            }
            
        }
        public async Task<IActionResult> OnPostVerifyEmailAsync() { 
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                CurrentUser = await _usersAppService.GetUserByIdAsync(userId);
                var orders = await _ordersAppService.GetListOrderByUserIdAsync(Guid.Parse(userId));
                Orders = orders;
                await VerifyOTPAsync(userId, OTP);
                
            }
            return RedirectToPage();
        }
        private async Task SendEmailConfirmAsync(string userId)
        {
            var user = await _usersAppService.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(Microsoft.AspNetCore.Identity.IdentityUser), userId);
            }
            //userdto to updatedto
            var otp = GenerateOTP();
            var otpExpiry = DateTime.Now.AddMinutes(10).ToString("o"); // OTP expires in 10 minutes

            var updateUser = new UpdateUserDto
            {
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                OTP = otp,
                OTPExpire = otpExpiry
            };
            await _usersAppService.UpdateOTPAsync(Guid.Parse(userId), updateUser);

            // Chuẩn bị dữ liệu cho template
            var model = new
            {
                message = otp
            };

            // Render template
            var emailBody = await _templateRenderer.RenderAsync(EmailTemplates.ConfirmEmail, model);

            // Gửi email
            await _emailSender.SendAsync(user.Email, "Xác thực OTP", emailBody);
        }

        private string GenerateOTP()
        {
            // You can use any preferred method to generate OTP
            // Here is a simple example
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task<IActionResult> VerifyOTPAsync(string userId, string otp)
        {
            var user = await _usersAppService.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(Microsoft.AspNetCore.Identity.IdentityUser), userId);
            }

            if (DateTime.TryParse(user.OTPExpire, out var expirationTime))
            {
                if (user.OTP == otp && expirationTime > DateTime.Now)
                {
                    // OTP hợp lệ, cập nhật trạng thái người dùng
                    var updateUser = new UpdateUserDto
                    {
                        EmailConfirmed = true,
                        OTP = null,
                        OTPExpire = null

                    };
                    await _usersAppService.UpdateOTPAsync(Guid.Parse(userId), updateUser);

                    return Page();
                }
            }
            else
            {
                // Invalid or expired OTP
                StatusMessage = "OTP không hợp lệ hoặc đã hết hạn.";
                return Page();
            }
            return Page();
        }


    }


}
