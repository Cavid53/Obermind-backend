using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(e => e.ProductName).IsRequired().HasMaxLength(150);
            builder.Property(e => e.ProductDescription).IsRequired().HasMaxLength(500);
            builder.Property(e => e.Quantity).IsRequired();
            builder.Property(e => e.ProductPrice).IsRequired();

            builder.HasOne(e => e.Order)
              .WithMany(e => e.OrderItems)
              .HasForeignKey(e => e.OrderId);

            builder.ToTable("OrderItems");
        }
    }
}
