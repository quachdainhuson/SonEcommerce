using System;
using System.Collections.Generic;
using System.Text;

namespace SonEcommerce.Public.Orders
{
    public class OrderEmailViewModel
    {
        public OrderDto Order { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
