using Core.Entities.OrderAggregate;
namespace API.Helpers;
internal sealed class MapsterConfigurationsBuilder
{
    private readonly IConfiguration _configuration;
    private readonly TypeAdapterConfig _mapsterConfig;
    public MapsterConfigurationsBuilder(IConfiguration configuration, TypeAdapterConfig mapsterConfigurations)
    {
        _configuration = Guard.Against.Null(configuration, nameof(configuration));
        _mapsterConfig = mapsterConfigurations;
    }
    public MapsterConfigurationsBuilder MapOrderItem()
    {
        _mapsterConfig.NewConfig<OrderItem, OrderItemDto>()
              .Map(dest => dest.ProductId, src => src.ItemOrdered.ProductItemId)
              .Map(dest => dest.ProductName, src => src.ItemOrdered.ProductName)
              .Map(dest => dest.PictureUrl, src => Expression(src.ItemOrdered))
              .TwoWays();
        return this;
    }
    public MapsterConfigurationsBuilder MapOrderToOrderDto()
    {
        _mapsterConfig.NewConfig<Order, OrderToReturnDto>()
                .Map(dest => dest.DeliveryMethod, src => src.DeliveryMethod.ShortName)
                .Map(dest => dest.ShippingPrice, src => src.DeliveryMethod.Price)
                .Map(dest => dest.Total, src => src.GetTotal())
                .TwoWays();
        return this;
    }

    public MapsterConfigurationsBuilder MapAddressToAddressDto<Tsource, TDestination>()
    {
        _mapsterConfig.NewConfig<Tsource, TDestination>()
              .IgnoreNullValues(true)
              .TwoWays();
        return this;
    }

    public MapsterConfigurationsBuilder MapProductToProductToReturnDto()
    {
        _mapsterConfig.NewConfig<Product, ProductToReturnDto>()
              .Map(dest => dest.ProductType, src => src.ProductType.Name)
              .Map(dest => dest.ProductBrand, src => src.ProductBrand.Name)
              .Map(dest => dest.PictureUrl, src => Expression(src))
              .TwoWays();
        return this;
    }

    public TypeAdapterConfig Build() => _mapsterConfig;

    private string Expression<TSource>(TSource source)
        where TSource : class
    {
        return Guard.Against.NullOrEmptyObject(source, _configuration);
    }


}

