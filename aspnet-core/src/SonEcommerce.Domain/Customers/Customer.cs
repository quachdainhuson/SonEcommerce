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
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
