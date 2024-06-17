using System;
using System.Collections.Generic;
using System.Text;

namespace SonEcommerce.Public.Users
{
    public class UpdateUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? UserAddress { get; set; }
        public string? UserCity { get; set; }
        public string? UserDistrict { get; set; }
        public string? UserWard { get; set; }
    }
}
