using Core.Entities.OrderAggregate;
using Address = Core.Entities.OrderAggregate.Address;
using Order = Core.Entities.OrderAggregate.Order;
namespace API.Controllers;
[Authorize]
public class OrdersController : BaseApiController
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(IOrderService orderService, IMapper mapper)
    {
        _orderService = Guard.Against.Null(orderService, nameof(orderService));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
    }
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
    {
        var email = HttpContext.User.RetrieveEmailFromPrincipal();
        var address = (Address)orderDto.ShipToAddress;
        var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);
        if (order is null)
        {
            return BadRequest(new ApiResponse(400, "Problem Creating Order"));
        }

        return Ok(order);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
    {
        var email = HttpContext.User.RetrieveEmailFromPrincipal();
        var orders = await _orderService.GetOrdersForUserAsync(email);
        var orderToReturn = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
        return Ok(orderToReturn);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
    {
        var email = HttpContext.User.RetrieveEmailFromPrincipal();
        var order = await _orderService.GetOrderByIdAsync(id, email);
        if (order is null)
        {
            return NotFound(new ApiResponse(404));
        }
        var orderToReturn = _mapper.Map<OrderToReturnDto>(order);
        return Ok(orderToReturn);
    }

    [HttpGet("deliveryMethods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
    {
        return Ok(await _orderService.GetDeliveryMethodsAsync());
    }
}
