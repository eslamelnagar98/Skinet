namespace API.Helpers;
using Address = Core.Entities.OrderAggregate.Address;
internal class MapsterProfile : IRegister
{
    private readonly IConfiguration _configuration;
    public MapsterProfile(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void Register(TypeAdapterConfig config)
    {
        new MapsterConfigurationsBuilder(_configuration, config)
           .MapOrderToOrderDto()
           .MapOrderItem()
           .MapAddressToAddressDto<Core.Entities.Identity.Address, AddressDto>()
           .MapAddressToAddressDto<Address, AddressDto>()
           .MapProductToProductToReturnDto()
           .Build();
    }
}
