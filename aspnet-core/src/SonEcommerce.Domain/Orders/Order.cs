﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace SonEcommerce.Orders
{
    public class Order : FullAuditedAggregateRoot<Guid>
    {
        public Order()
        {
        }
        public Order(Guid id)
        {
            Id = id;
        }
        public string? Code { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public double? ShippingFee { get; set; }
        public double? Tax { get; set; }
        public double? Total { get; set; }
        public double? Subtotal { get; set; }
        public double? Discount { get; set; }
        public double? GrandTotal { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public string? CustomerAddress { get; set; }
        public string? UserNote { get; set; }
        public string? UserCity { get; set; }
        public string? UserDistrict { get; set; }
        public string? UserWard { get; set; }
        public Guid? CustomerUserId { get; set; }
    }
}
