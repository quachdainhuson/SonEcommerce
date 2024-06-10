using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SonEcommerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SonEcommerce.Customers
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(SonEcommerceConsts.DbTablePrefix + "Customer");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                 .HasMaxLength(200);

            builder.Property(x => x.Email)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Phone)
                .HasMaxLength(50);

            builder.Property(x => x.Address)
                .HasMaxLength(250);

            builder.Property(x => x.Username)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Password)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
