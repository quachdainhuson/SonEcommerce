using System;
using System.Collections.Generic;
using System.Text;

namespace SonEcommerce.Public.Products
{
    public class ProductAttributeListFilterDto : BaseListFilterDto
    {
        public Guid ProductId { get; set; }
    }
}
