using System;
using System.Collections.Generic;
using System.Text;

namespace SonEcommerce.Admin.Products
{
    public class ProductAttributeListFilterDto : BaseListFilterDto
    {
        public Guid ProductId { get; set; }
    }
}
