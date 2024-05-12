using System;
using System.Collections.Generic;
using System.Text;

namespace SonEcommerce.Orders
{
    public enum TransactionType
    {
        ConfirmOrder,
        StartProcessing,
        FinishOrder,
        CancelOrder
    }
}
