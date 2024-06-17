using SonEcommerce.Orders;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SonEcommerce.Public.Orders
{
    public class OrderDto : EntityDto<Guid>
    {
        public string Code { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public double ShippingFee { get; set; }
        public double Tax { get; set; }
        public double Total { get; set; }
        public double Subtotal { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime CreationTime { get; set; }
        public string UserCity { get; set; }
        public string UserDistrict { get; set; }
        public string UserWard { get; set; }

        public Guid? CustomerUserId { get; set; }
        public Guid Id { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }

    }
}
