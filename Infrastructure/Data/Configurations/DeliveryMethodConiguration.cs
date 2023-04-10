namespace Infrastructure.Data.Configurations;
public class DeliveryMethodConiguration : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> deliveryMethodConfiguration)
    {
        deliveryMethodConfiguration.Property(deliveryMethod => deliveryMethod.Price)
                                   .HasColumnType("decimal(18,2)");
    }
}
