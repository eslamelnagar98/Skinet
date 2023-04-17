using AutoMapper;
using Core.Entities.OrderAggregate;
namespace API.Helpers;
public class OrderItemUrlResolver: IValueResolver<OrderItem, OrderItemDto, string>
{
    private readonly IConfiguration _configuration;
    public OrderItemUrlResolver(IConfiguration configuration)
    {
        _configuration = Guard.Against.Null(configuration, nameof(configuration));
    }

    public string Resolve(OrderItem orderItem, OrderItemDto orderItemDto, string destMember, ResolutionContext context)
    {
        return Guard.Against.NullOrEmptyObject(orderItem.ItemOrdered, _configuration);
    }
}


