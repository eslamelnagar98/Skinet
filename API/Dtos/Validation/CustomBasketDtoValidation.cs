namespace API.Dtos.Validation;
public class CustomBasketDtoValidation : AbstractValidator<CustomerBasketDto>
{
	public CustomBasketDtoValidation()
	{
		RuleFor(custome => custome.Id)
			.NotEmpty();

        RuleForEach(x => x.BasketItems).SetValidator(new BasketItemDtoValidation());
    }
}
