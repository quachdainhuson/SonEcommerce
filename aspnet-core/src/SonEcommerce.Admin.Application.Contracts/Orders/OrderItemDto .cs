using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using SonEcommerce.Admin.Products;

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
