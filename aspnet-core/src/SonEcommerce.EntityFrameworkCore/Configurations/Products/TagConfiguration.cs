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
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable(SonEcommerceConsts.DbTablePrefix + "Tags");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
             .HasMaxLength(50)
             .IsRequired();
            builder.Property(x => x.Name)
               .HasMaxLength(50)
               .IsRequired();

        }
    }
}
