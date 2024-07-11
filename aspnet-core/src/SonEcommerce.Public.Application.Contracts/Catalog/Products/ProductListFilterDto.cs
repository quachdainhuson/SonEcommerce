using System;
using System.Collections.Generic;
using System.Text;

namespace SonEcommerce.Public.Products
{
    public class ProductListFilterDto : BaseListFilterDto
    {
        public Guid? CategoryId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public double? MinPrice { get; set; } // Thêm thuộc tính MinPrice
        public double? MaxPrice { get; set; } // Thêm thuộc tính MaxPrice
    }
}
