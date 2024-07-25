using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SonEcommerce.Public.Users
{
    public class UpdateUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        public string PhoneNumber { get; set; }
        public string? UserAddress { get; set; }
        public string? UserCity { get; set; }
        public string? UserDistrict { get; set; }
        public string? UserWard { get; set; }
        public string? OTP { get; set; }
        public string? OTPExpire { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
