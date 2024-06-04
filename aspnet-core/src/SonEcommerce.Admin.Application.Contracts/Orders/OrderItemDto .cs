﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SonEcommerce.Admin
{
    public class OrderItemDto : EntityDto
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
