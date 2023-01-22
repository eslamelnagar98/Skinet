namespace API.Dtos;
public class CustomerBasketDto
{
    public string Id { get; set; }
    public List<BasketItemDto> BasketItems { get; set; }

    public static explicit operator CustomerBasketDto(CustomerBasket customerBasket)
    {
        return new()
        {
            Id = customerBasket.Id,
            BasketItems = customerBasket.BasketItems.Select(basketItem => (BasketItemDto)basketItem).ToList()
        };
    }

    public static explicit operator CustomerBasket(CustomerBasketDto customerBasketDto)
    {
        return new()
        {
            Id = customerBasketDto.Id,
            BasketItems = customerBasketDto.BasketItems.Select(basketItemDto => (BasketItem)basketItemDto).ToList()
        };
    }
}
