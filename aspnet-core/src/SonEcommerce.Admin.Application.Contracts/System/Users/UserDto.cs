using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SonEcommerce.Admin.System.Users
{
    public class UserDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }
        public string Surname { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải là 10 số.")]
        public string PhoneNumber { get; set; }
        public IList<string> Roles { get; set; }
        public bool IsActive { get; set; }
    }
}
