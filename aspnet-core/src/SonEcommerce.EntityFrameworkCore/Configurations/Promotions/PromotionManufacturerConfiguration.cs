using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SonEcommerce.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonEcommerce.Promotions
{
    public class PromotionManufacturerConfiguration : IEntityTypeConfiguration<PromotionManufacturer>
    {
        public void Configure(EntityTypeBuilder<PromotionManufacturer> builder)
        {
            builder.ToTable(SonEcommerceConsts.DbTablePrefix + "PromotionManufacturers");
            builder.HasKey(x => x.Id);
        }
    }
}
