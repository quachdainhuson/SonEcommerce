using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SonEcommerce.InventoryTickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonEcommerce.InventoryTickets
{
    public class InventoryTicketItemConfiguration : IEntityTypeConfiguration<InventoryTicketItem>
    {
        public void Configure(EntityTypeBuilder<InventoryTicketItem> builder)
        {
            builder.ToTable(SonEcommerceConsts.DbTablePrefix + "InventoryTicketItems");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.SKU)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(x => x.BatchNumber)
               .HasMaxLength(50)
               .IsUnicode(false);
        }
    }
}
