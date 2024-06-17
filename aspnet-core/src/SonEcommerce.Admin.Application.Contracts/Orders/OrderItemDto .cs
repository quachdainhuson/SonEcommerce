using System;
using System.Collections.Generic;
using System.Text;
using SonEcommerce.Admin.Products;
using Volo.Abp.Application.Dtos;

namespace SonEcommerce.Admin
{
    public class OrderItemDto : EntityDto
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public ProductDto Product { get; set; }
    }
}
