using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SonEcommerce.Admin.System.Users
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải là 10 số.")]
        public string PhoneNumber { get; set; }
    }
}
