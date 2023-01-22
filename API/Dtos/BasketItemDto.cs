namespace API.Dtos;
public class BasketItemDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string PictureUrl { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public static explicit operator BasketItemDto(BasketItem basketItem)
    {
        return new()
        {
            Brand = basketItem.Brand,
            Id = basketItem.Id,
            ProductName = basketItem.ProductName,
            Price = basketItem.Price,
            Quantity = basketItem.Quantity,
            PictureUrl = basketItem.PictureUrl,
            Type = basketItem.Type
        };
    }

    public static explicit operator BasketItem(BasketItemDto basketItemDto)
    {
        return new()
        {
            Brand = basketItemDto.Brand,
            Id = basketItemDto.Id,
            ProductName = basketItemDto.ProductName,
            Price = basketItemDto.Price,
            Quantity = basketItemDto.Quantity,
            PictureUrl = basketItemDto.PictureUrl,
            Type = basketItemDto.Type
        };
    }
}
