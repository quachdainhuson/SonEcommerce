using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace SonEcommerce.Customers
{
    public class Customer : FullAuditedEntity<Guid>
    {
        public Customer()
        {
        }
        public Customer(Guid id, string name, string email, string phone, string address, string username, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            Username = username;
            Password = password;
        }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
