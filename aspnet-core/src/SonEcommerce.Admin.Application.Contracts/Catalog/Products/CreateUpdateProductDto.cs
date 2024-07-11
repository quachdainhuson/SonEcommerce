using SonEcommerce.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace SonEcommerce.Admin.Products
{
    public class CreateUpdateProductDto
    {
        public Guid ManufacturerId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Slug { get; set; }
        public string SKU { get; set; }
        public bool Visibility { get; set; }
        public bool IsActive { get; set; }
        public double sellPrice { get; set; }
        public Guid CategoryId { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailPictureName { get; set; }
        public string? ThumbnailPictureContent { get; set; }
    }
}
