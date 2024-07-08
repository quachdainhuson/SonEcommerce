using SonEcommerce.Products;
using SonEcommerce.Admin.Manufacturers;
using SonEcommerce.Admin.ProductCategories;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SonEcommerce.Admin.Products
{
    public class ProductInListDto : EntityDto<Guid>
    {
        public Guid? ManufacturerId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Slug { get; set; }
        public ProductType ProductType { get; set; }
        public string? SKU { get; set; }
        public int? SortOrder { get; set; }
        public bool? Visibility { get; set; }
        public bool? IsActive { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime? CreationTime { get; set; }
        public string? ThumbnailPicture { get; set; }
        public string? CategoryName { get; set; }
        public string? CategorySlug { get; set; }
        public string? ManufacturerName { get; set; }

    }
}
