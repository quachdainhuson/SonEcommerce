using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SonEcommerce.Public.Users
{
    public class UserDto : AuditedEntityDto<Guid>
    {
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserAddress { get; set; }
        public string? UserCity { get; set; }
        public string? UserDistrict { get; set; }
        public string? UserWard { get; set; }
        public IList<string> Roles { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator List<object>(UserDto v)
        {
            throw new NotImplementedException();
        }
    }
}
