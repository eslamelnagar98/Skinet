namespace API.Dtos;
public class CustomerBasketDto
{
    public string Id { get; set; }
    public List<BasketItemDto> BasketItems { get; set; }
    public int? DeliveryMethod { get; set; }
    public string ClientSecret { get; set; }
    public string PaymentIntentId { get; set; }

    public static explicit operator CustomerBasketDto(CustomerBasket customerBasket)
    {
        return new()
        {
            Id = customerBasket.Id,
            DeliveryMethod = customerBasket.DeliveryMethod,
            ClientSecret = customerBasket.ClientSecret,
            PaymentIntentId = customerBasket.PaymentIntentId,
            BasketItems = customerBasket.BasketItems.Select(basketItem => (BasketItemDto)basketItem).ToList()
        };
    }

    public static explicit operator CustomerBasket(CustomerBasketDto customerBasketDto)
    {
        return new()
        {
            Id = customerBasketDto.Id,
            DeliveryMethod = customerBasketDto.DeliveryMethod,
            ClientSecret = customerBasketDto.ClientSecret,
            PaymentIntentId = customerBasketDto.PaymentIntentId,
            BasketItems = customerBasketDto.BasketItems.Select(basketItemDto => (BasketItem)basketItemDto).ToList()
        };
    }
}
