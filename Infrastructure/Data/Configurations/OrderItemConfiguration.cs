namespace Infrastructure.Data.Configurations;
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> orderItemBuilder)
    {
        orderItemBuilder.OwnsOne(orderItem => orderItem.ItemOrdered, orderItem => orderItem.WithOwner());
        orderItemBuilder.Property(orderItem => orderItem.Price)
                        .HasColumnType("decimal(18,2)");
    }
}
