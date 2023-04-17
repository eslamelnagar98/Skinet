using Mapster;
namespace API.Helpers;
using Address = Core.Entities.OrderAggregate.Address;
internal class MapsterProfile : IRegister
{
    private readonly MapsterConfigurationsBuilder _mapsterConfigurationBuilder;
    public MapsterProfile(IConfiguration configuration)
    {
        _mapsterConfigurationBuilder = new(configuration);
    }
    public void Register(TypeAdapterConfig config)
    {
        _mapsterConfigurationBuilder
            .MapOrderToOrderDto(config)
            .MapOrderItem(config)
            .MapAddressToAddressDto<Core.Entities.Identity.Address, AddressDto>(config)
            .MapAddressToAddressDto<Address, AddressDto>(config);
    }
}
