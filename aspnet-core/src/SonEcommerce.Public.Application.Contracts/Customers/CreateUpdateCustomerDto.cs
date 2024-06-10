using System;
using System.Collections.Generic;
using System.Text;

namespace SonEcommerce.Public
{
    public class CreateUpdateCustomerDto
    {
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
