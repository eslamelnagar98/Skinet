namespace API.Dtos.Validation;
public class AddressDtoValidation : AbstractValidator<AddressDto>
{
    public AddressDtoValidation()
    {
        RuleFor(address => address.FirstName)
            .NotEmpty();
        RuleFor(address => address.LastName)
            .NotEmpty();
        RuleFor(address => address.Street)
            .NotEmpty();
        RuleFor(address => address.City)
            .NotEmpty();
        RuleFor(address => address.State)
            .NotEmpty();
        RuleFor(address => address.ZipCode)
            .NotEmpty();

    }
}

