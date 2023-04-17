using Core.Entities.OrderAggregate;
namespace API.Helpers;
internal sealed class MapsterConfigurationsBuilder
{
    private readonly IConfiguration _configuration;
    public MapsterConfigurationsBuilder(IConfiguration configuration)
    {
        _configuration = Guard.Against.Null(configuration, nameof(configuration));
    }
    public MapsterConfigurationsBuilder MapOrderItem(TypeAdapterConfig config)
    {
        config.NewConfig<OrderItem, OrderItemDto>()
              .Map(dest => dest.ProductId, src => src.ItemOrdered.ProductItemId)
              .Map(dest => dest.ProductName, src => src.ItemOrdered.ProductName)
              .Map(dest => dest.PictureUrl, src => Expression(src.ItemOrdered))
              .TwoWays();
        return this;
    }
    public MapsterConfigurationsBuilder MapOrderToOrderDto(TypeAdapterConfig config)
    {
        config.NewConfig<Order, OrderToReturnDto>()
                .Map(dest => dest.DeliveryMethod, src => src.DeliveryMethod.ShortName)
                .Map(dest => dest.ShippingPrice, src => src.DeliveryMethod.Price)
                .Map(dest => dest.Total, src => src.GetTotal())
                .TwoWays();
        return this;
    }

    public MapsterConfigurationsBuilder MapAddressToAddressDto<Tsource, TDestination>(TypeAdapterConfig config)
    {
        config.NewConfig<Tsource, TDestination>()
              .IgnoreNullValues(true)
              .TwoWays();
        return this;
    }

    public MapsterConfigurationsBuilder MapProductToProductToReturnDto(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductToReturnDto>()
              .Map(dest => dest.ProductType, src => src.ProductType.Name)
              .Map(dest => dest.ProductBrand, src => src.ProductBrand.Name)
              .Map(dest => dest.PictureUrl, src => Expression(src));
        return this;
    }

    private string Expression<TSource>(TSource source)
        where TSource : class
    {
        return Guard.Against.NullOrEmptyObject(source, _configuration);
    }


}

