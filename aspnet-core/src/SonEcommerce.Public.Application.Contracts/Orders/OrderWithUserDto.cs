using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity;

namespace SonEcommerce.Public.Orders
{
    public class OrderWithUserDto
    {
        public OrderInListDto Order { get; set; }
        public IdentityUserDto User { get; set; }
    }
}
