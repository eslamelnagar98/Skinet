using Address = Core.Entities.OrderAggregate.Address;
using Order = Core.Entities.OrderAggregate.Order;
namespace Infrastructure.Services;
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBasketRepository _basketRepository;

    public OrderService(IUnitOfWork unitOfWork,
                        IBasketRepository basketRepository)
    {
        _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
        _basketRepository = Guard.Against.Null(basketRepository, nameof(basketRepository));
    }
    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
        var basket = await _basketRepository.GetBasketAsync(basketId);
        var orderItems = await CreateOrderItemListFromBasket(basket.BasketItems).ToListAsync();
        var order = await PrepareOrder(orderItems, buyerEmail, shippingAddress, deliveryMethodId);
        await _unitOfWork.Repository<Order>().AddAsync(order);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return null;
        await _basketRepository.DeleteBasketAsync(basketId);
        return order;
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
    {
        var specification = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
        return await _unitOfWork.Repository<Order>().GetEntityWithSpecification(specification);
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        var specification = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
        return await _unitOfWork.Repository<Order>().ListAsync(specification);
    }

    private async IAsyncEnumerable<OrderItem> CreateOrderItemListFromBasket(List<BasketItem> basketItems)
    {
        foreach (var basketItem in basketItems)
        {
            var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(basketItem.Id);
            var itemOrderd = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
            var orderItem = new OrderItem(itemOrderd, productItem.Price, basketItem.Quantity);
            yield return orderItem;
        }
    }

    private async Task<Order> PrepareOrder(IReadOnlyList<OrderItem> orderItems, string email, Address shippingAddress, int deliveryMethodId)
    {
        var subTotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);
        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
        return new Order(orderItems, email, shippingAddress, deliveryMethod, subTotal);
    }
}
