namespace API.Controllers;
public class BasketController : BaseApiController
{
    private readonly IBasketRepository _basketRepository;
    public BasketController(IBasketRepository basketRepository)
    {
        _basketRepository = Guard.Against.Null(basketRepository, nameof(basketRepository));
    }

    [HttpGet]
    public async Task<ActionResult<CustomerBasket>> GetBasketById(string basketId)
    {
        var basket = await _basketRepository.GetBasketAsync(basketId);
        return Ok(basket ?? new CustomerBasket(basketId));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
    {
        var customerBasket = (CustomerBasket)basket;
        var updatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);
        return Ok(updatedBasket);
    }

    [HttpDelete]
    public async Task DeleteBasketAsync(string basketId)
    {
        await _basketRepository.DeleteBasketAsync(basketId);
    }
}
