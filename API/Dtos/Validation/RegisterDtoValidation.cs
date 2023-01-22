namespace API.Dtos.Validation;
public class RegisterDtoValidation : AbstractValidator<RegisterDto>
{
	public RegisterDtoValidation()
	{
		RuleFor(register => register.DisplayName)
			.NotEmpty();
        RuleFor(register => register.Email)
            .EmailAddress();
        RuleFor(register => register.Password)
            .NotEmpty()
            .Matches(@"(?=^.{6,10}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$")
            .WithMessage("Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 6 characters");
    }
}
