using System;
using System.Collections.Generic;
using System.Text;

namespace SonEcommerce.Public.Users
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string PhoneNumber { get; set; }
        public string? UserAddress { get; set; }
    }
}
