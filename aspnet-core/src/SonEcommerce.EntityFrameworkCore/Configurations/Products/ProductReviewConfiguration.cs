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
    public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> builder)
        {
            builder.ToTable(SonEcommerceConsts.DbTablePrefix + "ProductReviews");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title)
               .HasMaxLength(250)
               .IsRequired();

        }
    }
}
