using Address = Core.Entities.OrderAggregate.Address;
namespace Core.Interfaces;
public interface IOrderService
{
    Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress);
    Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
    Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    Task<Order> GetOrderByIdAsync(int id, string buyerEmail);
}
