namespace Infrastructure.Data.Configurations;
public class OrderConfiguration : IEntityTypeConfiguration<Core.Entities.OrderAggregate.Order>
{
    public void Configure(EntityTypeBuilder<Core.Entities.OrderAggregate.Order> orderBuilder)
    {
        orderBuilder.OwnsOne(order => order.ShipToAddress, a =>
        {
            a.WithOwner();
        });

        orderBuilder.Property(order => order.Status)
                    .HasConversion(orderStatus => orderStatus.ToString(),
                     value => (OrderStatus)Enum.Parse(typeof(OrderStatus), value));

        orderBuilder.HasMany(order => order.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}

