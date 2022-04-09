using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(e => e.No).IsRequired().HasMaxLength(150);
            builder.Property(e => e.TotalPrice).IsRequired(false);
            builder.Property(e => e.OrderDate).IsRequired();
            builder.Property(e => e.Status).IsRequired().HasDefaultValue(OrderStatus.Draft);

            builder.ToTable("Orders");
        }
    }
}
