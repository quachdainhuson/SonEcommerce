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
    public class InventoryTicketConfiguration : IEntityTypeConfiguration<InventoryTicket>
    {
        public void Configure(EntityTypeBuilder<InventoryTicket> builder)
        {
            builder.ToTable(SonEcommerceConsts.DbTablePrefix + "InventoryTickets");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
        }
    }
}
