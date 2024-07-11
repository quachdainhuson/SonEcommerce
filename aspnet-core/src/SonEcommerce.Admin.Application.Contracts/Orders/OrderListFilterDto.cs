using SonEcommerce.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SonEcommerce.Admin.Orders
{
    public class OrderListFilterDto : BaseListFilterDto
    {
        public OrderStatus? Status { get; set; }
    }
}
