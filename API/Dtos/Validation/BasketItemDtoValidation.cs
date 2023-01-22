namespace API.Dtos.Validation;
public class BasketItemDtoValidation : AbstractValidator<BasketItemDto>
{
    public BasketItemDtoValidation()
    {
        RuleFor(basket => basket.Id)
            .NotEmpty();

        RuleFor(basket => basket.Quantity)
            .NotEmpty();

        RuleFor(basket => basket.Brand)
           .NotEmpty();

        RuleFor(basket => basket.ProductName)
           .NotEmpty();

        RuleFor(basket => basket.PictureUrl)
           .NotEmpty();

        RuleFor(basket => basket.Type)
          .NotEmpty();

        RuleFor(basket => basket.Price)
           .InclusiveBetween(0.1m, decimal.MaxValue)
           .WithMessage($"Price must be number Beetween 0.1 , {decimal.MaxValue}");

        RuleFor(basket => basket.Quantity)
            .InclusiveBetween(1, int.MaxValue)
            .WithMessage($"Quantity must be number Beetween 1 , {int.MaxValue}");

    }
}
