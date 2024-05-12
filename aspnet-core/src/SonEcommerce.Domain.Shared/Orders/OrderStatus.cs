using System;
using System.Collections.Generic;
using System.Text;

namespace SonEcommerce.Orders
{
    public enum OrderStatus
    {
        New,
        Confirmed,
        Processing,
        Shipping,
        Finished,
        Canceled
    }
}
