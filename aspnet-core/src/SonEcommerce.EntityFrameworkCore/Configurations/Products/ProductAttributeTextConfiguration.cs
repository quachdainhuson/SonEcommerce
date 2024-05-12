using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SonEcommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonEcommerce.Products
{
    public class ProductAttributeTextConfiguration : IEntityTypeConfiguration<ProductAttributeText>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeText> builder)
        {
            builder.ToTable(SonEcommerceConsts.DbTablePrefix + "ProductAttributeTexts");
            builder.HasKey(x => x.Id);
        }
    }
}
